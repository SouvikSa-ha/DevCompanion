using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevCom.Models.ViewModels
{
    public class NotepadVM
    {
        /*
        public NotepadVM()
        {
            this.Notepad = new Notepad();
            this.Text = new Text();
            this.Image = new Image();
            this.Audio = new Audio();
            this.Video = new Video();
            this.Canvas = new Canvas();
        }*/

        public IEnumerable<Notepad> Notepads { get; set; }
        public IEnumerable<Text> Texts { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public IEnumerable<Audio> Audios { get; set; }
        public IEnumerable<Video> Videos { get; set; }
        public IEnumerable<File> Files { get; set; }
        public IEnumerable<Canvas> Canvases { get; set; }
        public IEnumerable<IEnumerable<string>> Content_ids { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public List<string> AllTags { get; set; }

        public string textsubstr = "t";
        public string imagesubstr = "i";
        public string audiosubstr = "a";
        public string videosubstr = "v";
        public string filesubstr = "f";
        public string canvassubstr = "c";

        /*
        public Notepad Notepad { get; set; }
        public Text Text { get; set; }
        public Image Image { get; set; }
        public Audio Audio { get; set; }
        public Video Video { get; set; }
        public File File { get; set; }
        public Canvas Canvas { get; set; }
        */
    }
}

