﻿namespace DVLD.Application.Models
{
    public class ImagesOptions
    {
        public string ImagesDirectory { get; set; }
        public List<string> AllowedExtensions { get; set; }
        public int MaxSizeInMegaBytes { get; set; }
    }
}
