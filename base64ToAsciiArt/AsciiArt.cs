/*======================================================================/

Copyright (C) 2004 Daniel Fisher(lennybacon).  All rights reserved.

THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
PARTICULAR PURPOSE.

For more information email: info@lennybacon.com

12/27/04 Enhancements by Steven Fowler (steven_m_fowler@yahoo.com)
=======================================================================*/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace StaticDust
{
	/// <summary>
	/// Summary description for image2html.
	/// </summary>
	public static class AsciiArt
	{
		public static string ConvertImage(Stream stream, string ImageSize, string Quick)
		{
			StringBuilder _asciiart = new StringBuilder();

			#region load image
			Image _img = Image.FromStream(stream);
			Bitmap _image = new Bitmap(_img, new Size(_img.Width, _img.Height));
			_img.Dispose();


			Rectangle bounds = new Rectangle(0, 0, _image.Width, _image.Height);
			#endregion

			#region greyscale image
			ColorMatrix _matrix = new ColorMatrix();

			_matrix[0,0] = 1/3f;
			_matrix[0,1] = 1/3f;
			_matrix[0,2] = 1/3f;
			_matrix[1,0] = 1/3f;
			_matrix[1,1] = 1/3f;
			_matrix[1,2] = 1/3f;
			_matrix[2,0] = 1/3f;
			_matrix[2,1] = 1/3f;
			_matrix[2,2] = 1/3f;

			ImageAttributes _attributes = new ImageAttributes();
			_attributes.SetColorMatrix(_matrix);


			Graphics gphGrey = Graphics.FromImage(_image);
			gphGrey.DrawImage(_image, bounds, 0, 0, _image.Width, _image.Height,
				GraphicsUnit.Pixel, _attributes);

			gphGrey.Dispose();
			#endregion

			#region ascii image

			int _pixwidth = 1;
			Int32.TryParse(ImageSize, out _pixwidth);
			if (_pixwidth < 1)
				_pixwidth = 1;

			int minSize = Math.Min(_image.Width, _image.Height);
			if (_pixwidth > minSize)
				_pixwidth = minSize;

			int _pixhight = _pixwidth * 2;
			int _pixseg = _pixwidth * _pixhight;

			for(int h=0; h < _image.Height / _pixhight; h++)
			{
				// segment hight
				int _startY = (h * _pixhight);
				// segment width
				for(int w = 0; w < _image.Width / _pixwidth; w++)
				{
					int _startX = (w *_pixwidth);
					int _allBrightness = 0;

					if(Quick == "True")
					{
						// each pix of this segment
						for(int y = 0; y < _pixwidth; y++)
						{
							try
							{
								Color _c = _image.GetPixel(_startX,y+_startY);
								int _b = (int)(_c.GetBrightness() * 100);
								_allBrightness = _allBrightness + _b;
							}
							catch
							{
								_allBrightness = (_allBrightness + 50);
							}
						}
					}
					else
					{
						// each pix of this segment
						for(int y = 0; y < _pixwidth; y++)
						{
							for(int x = 0; x < _pixhight; x++)
							{
								int _cY = y + _startY;
								int _cX = x + _startX;
								try
								{
									Color _c = _image.GetPixel(_cX, _cY);
									int _b = (int)(_c.GetBrightness() * 100);
									_allBrightness = _allBrightness + _b;
								}
								catch
								{
									_allBrightness = (_allBrightness + 50);
								}
							}
						}
					}

					int _sb = (_allBrightness / _pixseg);
					if (_sb < 10 )
					{
						_asciiart.Append("#");
					}
					else if (_sb < 17 )
					{
						_asciiart.Append("@");
					}
					else if (_sb < 24)
					{
						_asciiart.Append("&");
					}
					else if (_sb < 31)
					{
						_asciiart.Append("$");
					}
					else if (_sb < 38)
					{
						_asciiart.Append("%");
					}
					else if (_sb < 45)
					{
						_asciiart.Append("|");
					}
					else if (_sb < 52)
					{
						_asciiart.Append("!");
					}
					else if (_sb < 59)
					{
						_asciiart.Append(";");
					}
					else if (_sb < 66)
					{
						_asciiart.Append(":");
					}
					else if (_sb < 73)
					{
						_asciiart.Append("'");
					}
					else if (_sb < 80)
					{
						_asciiart.Append("`");
					}
					else if (_sb < 87)
					{
						_asciiart.Append(".");
					}
					else
					{
						_asciiart.Append(" ");
					}
				}
				_asciiart.Append("\n");
			}
			#endregion
			
			//clean up
			_image.Dispose();

			return _asciiart.ToString();

		}
	}
}