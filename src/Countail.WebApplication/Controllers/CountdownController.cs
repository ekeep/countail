using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using Countail.WebApplication.Utilities;

namespace Countail.WebApplication.Controllers
{
    public class CountdownController : Controller
    {
        [OutputCache(Duration = 5)]
        public ActionResult Gif(string id)
        {
            var font = new Font(new FontFamily("tahoma"), 20.3f);

            var backColor = Color.Black;
            var textColor = Color.White;

            var endDatetime = new DateTime(2016, 9, 21, 8, 0, 0);

            var image = GenerateCountdownGif(endDatetime, 180, font, backColor, textColor);

            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Gif);
            stream.Position = 0;

            return base.File(stream, "image/gif");
        }

        protected Image GenerateCountdownGif(DateTime tilDateTime, int secondsToShow, Font font, Color backColor, Color textColor)
        {
            var imagesList = new List<Image>();

            var now = DateTime.UtcNow;
            var diff = tilDateTime - now;
            int upperbound;
            if (diff.TotalSeconds > secondsToShow)
            {
                upperbound = secondsToShow;
            }
            else
            {
                upperbound = (int)diff.TotalSeconds;
            }

            for (var i = 0; i < upperbound; i++)
            {
                var text = $"{diff.Days:00}:{diff.Hours:00}:{diff.Minutes:00}:{diff.Seconds:00}";
                var image = GenerateImage(text, font, backColor, textColor);
                imagesList.Add(image);

                diff = diff.Add(new TimeSpan(0, 0, 0, -1));
            }

            return GenerateAnimatedImage(imagesList, 1);
        }

        protected Image GenerateAnimatedImage(List<Image> images, int duration)
        {
            var stream = new MemoryStream();
            var gifEncoder = new GifEncoder(stream) {FrameDelay = new TimeSpan(0, 0, 0, duration)};

            foreach (var image in images)
            {
                gifEncoder.AddFrame(image);
            }

            var animatedImage = Image.FromStream(stream);

            gifEncoder.Dispose();
            stream.Dispose();

            return animatedImage;
        }

        protected Image GenerateImage(string text, Font font, Color backColor, Color textColor)
        {
            var image = new Bitmap(1, 1);
            var drawing = Graphics.FromImage(image);

            var textSize = drawing.MeasureString(text, font);
            image.Dispose();
            drawing.Dispose();

            image = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(image);
            drawing.Clear(backColor);

            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);
            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return image;
        }
    }
}