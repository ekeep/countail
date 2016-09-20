using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;

namespace Countail.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 15)]
        public ActionResult MyGif(string id)
        {
            var font = new Font(new FontFamily("tahoma"), 20.3f);
            
            var backColor = Color.AntiqueWhite;
            var textColor = Color.Crimson;

            var tommorow = new DateTime(2016, 5, 14, 8, 0, 0).ToUniversalTime();

            var image = GenerateCountdownGif(tommorow, 180, font, backColor, textColor);

            //System.Drawing.Imaging.Encoder myEncoder = Encoder.Quality;
            //EncoderParameters myEncoderParameters = new EncoderParameters(1);
            //EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 200L);
            //myEncoderParameters.Param[0] = myEncoderParameter;

            var stream = new MemoryStream();
            //image.Save(stream, GetEncoder(ImageFormat.Gif), myEncoderParameters);
            image.Save(stream, ImageFormat.Gif);
            stream.Position = 0;

            return base.File(stream, "image/gif");
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="images">List of images</param>
        /// <param name="duration">duration between each frame in seconds</param>
        /// <returns></returns>
        protected Image GenerateAnimatedImage(List<Image> images, int duration)
        {
            MemoryStream stream = new MemoryStream();
            GifEncoder gifEncoder = new GifEncoder(stream);
            gifEncoder.FrameDelay = new TimeSpan(0, 0, 0, duration);

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
            //image.SetResolution(300,300);
            var drawing = Graphics.FromImage(image);

            var textSize = drawing.MeasureString(text, font);
            image.Dispose();
            drawing.Dispose();

            image = new Bitmap((int)textSize.Width, (int)textSize.Height);
            //image.SetResolution(300, 300);
            
            drawing = Graphics.FromImage(image);
            drawing.Clear(backColor);

            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);
            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return image;
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}