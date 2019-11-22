using System;
using System.Collections.Generic;

namespace Structurizr
{
    public class PaperSize
    {

        private static Dictionary<string, PaperSize> paperSizes = new Dictionary<string, PaperSize>();

        public static readonly PaperSize A6_Portrait = new PaperSize("A6_Portrait", "A6", Orientation.Portrait, 1240, 1748);
        public static readonly PaperSize A6_Landscape = new PaperSize("A6_Landscape", "A6", Orientation.Landscape, 1748, 1240);

        public static readonly PaperSize A5_Portrait = new PaperSize("A5_Portrait", "A5", Orientation.Portrait, 1748, 2480);
        public static readonly PaperSize A5_Landscape = new PaperSize("A5_Landscape", "A5", Orientation.Landscape, 2480, 1748);

        public static readonly PaperSize A4_Portrait = new PaperSize("A4_Portrait", "A4", Orientation.Portrait, 2480, 3508);
        public static readonly PaperSize A4_Landscape = new PaperSize("A4_Landscape", "A4", Orientation.Landscape, 3508, 2480);

        public static readonly PaperSize A3_Portrait = new PaperSize("A3_Portrait", "A3", Orientation.Portrait, 3508, 4961);
        public static readonly PaperSize A3_Landscape = new PaperSize("A3_Landscape", "A3", Orientation.Landscape, 4961, 3508);

        public static readonly PaperSize A2_Portrait = new PaperSize("A2_Portrait", "A2", Orientation.Portrait, 4961, 7016);
        public static readonly PaperSize A2_Landscape = new PaperSize("A2_Landscape", "A2", Orientation.Landscape, 7016, 4961);

        public static readonly PaperSize Letter_Portrait = new PaperSize("Letter_Portrait", "Letter", Orientation.Portrait, 2550, 3300);
        public static readonly PaperSize Letter_Landscape = new PaperSize("Letter_Landscape", "Letter", Orientation.Landscape, 3300, 2550);

        public static readonly PaperSize Legal_Portrait = new PaperSize("Legal_Portrait", "Legal", Orientation.Portrait, 2550, 4200);
        public static readonly PaperSize Legal_Landscape = new PaperSize("Legal_Landscape", "Legal", Orientation.Landscape, 4200, 2550);

        public static readonly PaperSize Slide_4_3 = new PaperSize("Slide_4_3", "Slide 4:3", Orientation.Landscape, 3306, 2480);
        public static readonly PaperSize Slide_16_9 = new PaperSize("Slide_16_9", "Slide 16:9", Orientation.Landscape, 3508, 1973);

        public string Key { get; }
        public String Name { get; }
        public Orientation Orientation { get; }
        public int width { get; }
        public int height { get; }

        private PaperSize(String key, String name, Orientation orientation, int width, int height)
        {
            this.Key = key;
            this.Name = name;
            this.Orientation = orientation;
            this.width = width;
            this.height = height;

            paperSizes.Add(key, this);
        }

        public static PaperSize GetPaperSize(string key)
        {
            PaperSize paperSize;
            if (key == null || !paperSizes.TryGetValue(key, out paperSize))
            {
                paperSize = A4_Portrait;
            }

            return paperSize;
        }

    }

    public enum Orientation
    {
        Portrait,
        Landscape
    }

}
