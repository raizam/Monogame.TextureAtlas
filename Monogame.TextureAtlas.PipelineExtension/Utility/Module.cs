
using System.Drawing;

namespace TextureAtlas.PipelineExtension.Utility
{
    public class Module
    {
        private int name;
        private int width;
        private int height;
        private int whiteSpace;
        private int xCoordinate;
        private int yCoordinate;
        private Bitmap image;

        /// <summary>
        /// Module class representing an image and it's size including white space around the image.
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_image"></param>
        /// <param name="_whiteSpace">Width of white space around the image.</param>
        public Module(int _name, Bitmap _image, int _whiteSpace)
        {
            name = _name;

            if (_image != null)
            {
                width = _image.Width + _whiteSpace;
                height = _image.Height + _whiteSpace;
            }
            //Empty module
            else
                width = height = 0;

            whiteSpace = _whiteSpace;
            xCoordinate = 0;
            yCoordinate = 0; 
            image = _image;
        }

        /// <summary>
        /// Gets the width of the module.
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// Gets the height of the module.
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the module's bottom left corner.
        /// </summary>
        public int X
        {
            get { return xCoordinate; }
            set { xCoordinate = value; }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the module's bottom left corner.
        /// </summary>
        public int Y
        {
            get { return yCoordinate; }
            set { yCoordinate = value; }
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        public int Name
        {
            get { return name; }
        }

        /// <summary>
        /// Sets coordinates of module to zero.
        /// </summary>
        public void ClearCoordinates()
        {
            xCoordinate = 0;
            yCoordinate = 0;
        }

        /// <summary>
        /// Deep copy.
        /// </summary>
        /// <returns></returns>
        public Module Copy()
        {
            Module copy = new Module(name, image, whiteSpace);
            copy.xCoordinate = xCoordinate;
            copy.yCoordinate = yCoordinate;
            return copy;
        }

        /// <summary>
        /// Draws the module into a graphics object.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="marginWidth">Margin width around the sprite.</param>
        public void Draw(Graphics graphics, int marginWidth)
        {
            graphics.DrawImage(image, xCoordinate + marginWidth, yCoordinate + marginWidth, 
                image.Width, image.Height);
        }
    }
}
