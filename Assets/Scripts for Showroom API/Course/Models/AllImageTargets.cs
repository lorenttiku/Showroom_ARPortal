using System;

namespace Assets.Course.Models
{
    [Serializable]
    public class AllImageTargets
    {
        public ImageTarget[] data;
        public Meta meta;
    }

    [Serializable]
    public class ImageTarget
    {
        public int id;
        public string AutorName;
        public string Description;
        public string PictureLink;
        
        //public string AuthorName { get; internal set; }
    }

    [Serializable]
    public class Meta
    {
        public string page;
    }
}