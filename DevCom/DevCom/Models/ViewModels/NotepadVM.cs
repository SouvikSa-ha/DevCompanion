using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevCom.Models.ViewModels
{
    public class NotepadVM
    {
        public IEnumerable<Notepad> Notepads { get; set; }
        public IEnumerable<Text> Texts { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public IEnumerable<Audio> Audios { get; set; }
        public IEnumerable<Video> Videos { get; set; }
        public IEnumerable<File> Files { get; set; }
        public IEnumerable<Canvas> Canvases { get; set; }

        public Notepad Notepad { get; set; }
        public Text Text { get; set; }
        public Image Image { get; set; }
        public Audio Audio { get; set; }
        public Video Video { get; set; }
        public File File { get; set; }
        public Canvas Canvas { get; set; }
    }
}

