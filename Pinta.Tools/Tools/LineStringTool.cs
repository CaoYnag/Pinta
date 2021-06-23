// 
// LineCurveTool.cs
//  
// Author:
//       Jonathan Pobst <monkey@jpobst.com>
// 
// Copyright (c) 2010 Jonathan Pobst
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Cairo;
using Pinta.Core;
using Mono.Unix;
using System.Collections.Generic;
using System.Linq;

namespace Pinta.Tools
{
	public class LineStringTool : ShapeTool
	{
		public override string Name
		{
			get { return Catalog.GetString("LineString"); }
		}
		public override string Icon {
			get { return "Tools.LineString.png"; }
		}
		public override string StatusBarText {
			get	{ return Catalog.GetString ("Left click to draw a shape with the primary color." +
					"\nLeft click on a shape to add a control point.");
			}
		}
		public override Gdk.Cursor DefaultCursor {
            get { return new Gdk.Cursor (Gdk.Display.Default, PintaCore.Resources.GetIcon ("Cursor.LineString.png"), 9, 18); }
		}
		public override int Priority {
			get { return 40; }
		}

		public override BaseEditEngine.ShapeTypes ShapeType
		{
			get
			{
				return BaseEditEngine.ShapeTypes.LineString;
			}
		}

        public LineStringTool ()
        {
			EditEngine = new LineCurveEditEngine(this);

			BaseEditEngine.CorrespondingTools.Add(ShapeType, this);
        }
	}
}
