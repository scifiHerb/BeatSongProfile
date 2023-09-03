using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using HMUI;
using System.Reflection;
using BeatSaberMarkupLanguage;
using BeatSongProfile.UI;
using Newtonsoft.Json;
using static IPA.Logging.Logger;
using BeatSaberMarkupLanguage.Components;
using BeatSongProfile.Tools;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace LevelDetailsViewer.HarmonyPatches
{
    [HarmonyPatch(typeof(StandardLevelDetailView))]
    [HarmonyPatch("SetContent", MethodType.Normal)]
    internal class LevelListTableCellSetDataFromLevel
    {
        public static IBeatmapLevel selectedLevel = null;
        public static Transform button = null;
        private static ClickableImage _artworkImage;
        private static StandardLevelDetailViewController _standardLevelDetailViewController;

        private static void Postfix(IBeatmapLevel level, BeatmapCharacteristicSO defaultBeatmapCharacteristic,
            PlayerData playerData, TextMeshProUGUI ____actionButtonText, StandardLevelDetailView __instance, IDifficultyBeatmap ____selectedDifficultyBeatmap)
        {
            selectedLevel = level;
            if (_artworkImage == null)
            {
                var a = __instance.transform.parent.GetComponent<StandardLevelDetailViewController>();

                var detailView = Accessors.DetailView(ref a);
                var levelBar = Accessors.LevelBar(ref detailView);
                var artwork = Accessors.Artwork(ref levelBar);

                // Upgrade the ImageView
                var clickable = artwork.Upgrade<ImageView, ClickableImage>();


                Accessors.Artwork(ref levelBar) = clickable;
                _artworkImage = clickable;
                _artworkImage.OnClickEvent += mainButtonClick;
            }

            profileUI.Create(__instance.gameObject);
            GetSongStats(level.levelID);
        }

        private static void mainButtonClick(PointerEventData p)
        {
            BeatSongProfile.Plugin.Log.Info("click");
            if (profileUI.instance == null) return;

            profileUI.instance.showDetail();

        }
        private static async void GetSongStats(string levelID)
        {
            //カスタムでない場合return
            if (levelID.IndexOf("custom_level") == -1) return;
            string url = "https://api.beatsaver.com/maps/hash/" + levelID.Substring(13);

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url); // 非同期にWebリクエストを送信する

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync(); // 非同期にレスポンスを読み込む

                LevelInformation data = JsonConvert.DeserializeObject<LevelInformation>(result);
                profileUI.instance.setDetails(data);

                GetMapperInfo(data.uploader.playlistUrl);
            }
        }
        private static async void GetMapperInfo(string url)
        {
            if (url.IndexOf("/playlist") < 0) return;

            url = url.Substring(0, url.IndexOf("/playlist"));

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url); // 非同期にWebリクエストを送信する

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync(); // 非同期にレスポンスを読み込む

                MapperInformation.User data = JsonConvert.DeserializeObject<MapperInformation.User>(result);
                profileUI.instance.setMapperDetails(data);

            }
        }
    }
}
