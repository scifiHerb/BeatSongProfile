using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static IPA.Logging.Logger;

namespace BeatSongProfile.UI
{
    internal class profileUI
    {
        static public profileUI instance = new profileUI();

        public static void Create(GameObject obj)
        {
            BSMLParser.instance.Parse(
                Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), $"BeatSongProfile.UI.profileUI.bsml"),
                obj, instance
            );
            instance.root.name = "songProfile";
        }

        public void setDetails(LevelInformation level)
        {
            if (artwork == null) return;

            //set 
            if (level.versions[0] != null) artwork.SetImage(level.versions[0].coverURL);
            songText.text = $"<color=#00ffff><size=150%>{level.metadata.songName}</size></color>\n";
            if (level.metadata.songSubName != "") songText.text += $"<color=#00bfff>{level.metadata.songSubName}</color>\n";
            songText.text += $"({level.metadata.songAuthorName})\n" +
                $"hash[<color=#00FF00>{level.uploader.hash}</color>]\n" +
                $"BSR[<color=#00ff00>{level.id}</color>] - <color=#00ff00>{level.uploaded}</color>\n" +
                $"Votes[↑<color=#00ff00>{level.stats.upvotes}</color>:↓<color=#ff0000>{level.stats.downvotes}</color>]";

            description.text = level.description;

            mapperIcon.SetImage(level.uploader.avatar);
        }

        public void setMapperDetails(MapperInformation.User mapper)
        {
            if (artwork == null) return;

            //Plugin.Log.Info(mapper.name);
            mapperText.text = $"<color=#00ffff><size=150%>{mapper.name}</size></color>\n" +
                $"hash[<color=#00FF00>{mapper.hash}</color>]\n" +
                $"totalVotes[<color=#00FF00>{mapper.stats.totalUpvotes}</color>,<color=#FF0000>{mapper.stats.totalDownvotes}</color>]\n" +
                $"totalMaps[<color=#00FF00>{mapper.stats.totalMaps}</color>]\n" +
                $"[<color=#00FF00>{mapper.stats.firstUpload}</color> - <color=#00FF00>{mapper.stats.lastUpload}</color>]";


            mapperDescription.text = mapper.description;
        }

        public void resetView()
        {
            artwork.SetImage("");
            mapperIcon.SetImage("");
            songText.text = "";
            mapperText.text = "";
            description.text = "";
            mapperDescription.text = "";
        }
        public void log(string str)
        {
            Plugin.Log.Info(str);
        }

        public void showDetail()
        {
            //debug ui position settings
            parserParams?.EmitEvent("show-detail");
        }
        [UIAction("onClick_artwork")]
        protected async Task ViewArtwork()
        {
            parserParams?.EmitEvent("hide");

            await SiraUtil.Extras.Utilities.PauseChamp;
            parserParams?.EmitEvent("show-artwork");
        }

        [UIAction("close-submodal")]
        protected async Task Close()
        {
            parserParams?.EmitEvent("hide");
            await SiraUtil.Extras.Utilities.PauseChamp;
            parserParams?.EmitEvent("show-detail");
        }

        [UIAction("close-window")]
        protected async Task CloseAll()
        {
            parserParams?.EmitEvent("hide");
            await SiraUtil.Extras.Utilities.PauseChamp;
        }

        [UIParams]
        protected BSMLParserParams parserParams = null;
        [UIComponent("main-modal")]
        protected RectTransform root = null;
        [UIComponent("artworkContainer")]
        protected RectTransform artworkContainer = null;


        [UIComponent("artwork")]
        protected ClickableImage artwork = null;
        [UIComponent("songText")]
        protected TextMeshProUGUI songText = null;
        [UIComponent("description")]
        protected TextMeshProUGUI description = null;

        [UIComponent("mapperIcon")]
        protected ClickableImage mapperIcon = null;
        [UIComponent("mapperText")]
        protected TextMeshProUGUI mapperText = null;
        [UIComponent("mapperDescription")]
        protected TextMeshProUGUI mapperDescription = null;
    }
}
