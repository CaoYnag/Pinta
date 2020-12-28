﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Pinta.Core;

namespace Pinta.Core.Tests
{
	[TestFixture]
	class TextEngineTest
	{
		// The string below contains combining characters, so there are fewer text elements than chars.
		private readonly List<string> testSnippet = new (){
			"a\u0304\u0308bc\u0327",
			"c\u0327ba\u0304\u0308",
			"bc\u0327a\u0304\u0308"
		};

		private string LinesToString (string[] lines) => string.Join (Environment.NewLine, lines);

		[Test]
		public void PerformEnter ()
		{
			var engine = new TextEngine (new List<string> () { "foo", "bar" });
			engine.SetCursorPosition (new TextPosition (1, 1), true);
			engine.PerformEnter ();

			Assert.AreEqual (3, engine.LineCount);
			Assert.AreEqual (LinesToString (new string[] { "foo", "b", "ar" }),
					 engine.ToString ());
			Assert.AreEqual (new TextPosition (2, 0), engine.CurrentPosition);
		}

		[Test]
		public void DeleteMultiLineSelection ()
		{
			var engine = new TextEngine (new List<string> () { "line 1", "line 2", "line 3" });
			engine.SetCursorPosition (new TextPosition (0, 2), true);
			engine.PerformDown (true);
			engine.PerformDown (true);
			engine.PerformDelete ();

			Assert.AreEqual (1, engine.LineCount);
			Assert.AreEqual (LinesToString (new string[] { "line 3" }),
					 engine.ToString ());
		}

		[Test]
		public void DeleteSelection ()
		{
			var engine = new TextEngine (new List<string> () { "это тест", "это еще один тест" });
			engine.SetCursorPosition (new TextPosition (0, 2), true);
			engine.PerformDown (true);
			engine.PerformDelete ();

			Assert.AreEqual (1, engine.LineCount);
			Assert.AreEqual (LinesToString (new string[] { "это еще один тест" }),
					 engine.ToString ());
			Assert.AreEqual (new TextPosition (0, 2), engine.CurrentPosition);
		}

		[Test]
		public void BackspaceJoinLines ()
		{
			var engine = new TextEngine (new () { "foo", "bar" });
			engine.SetCursorPosition (new TextPosition (1, 0), true);
			engine.PerformBackspace ();

			Assert.AreEqual (1, engine.LineCount);
			Assert.AreEqual ("foobar", engine.ToString ());
			Assert.AreEqual (new TextPosition (0, 3), engine.CurrentPosition);
		}

		[Test]
		public void Backspace ()
		{
			var engine = new TextEngine (testSnippet);

			// End of a line.
			engine.SetCursorPosition (new TextPosition (0, 6), true);
			engine.PerformBackspace ();

			Assert.AreEqual ("a\u0304\u0308b", engine.Lines[0]);
			Assert.AreEqual (new TextPosition (0, 4), engine.CurrentPosition);

			// First character of a line.
			engine.SetCursorPosition (new TextPosition (1, 2), true);
			engine.PerformBackspace ();

			Assert.AreEqual ("ba\u0304\u0308", engine.Lines[1]);
			Assert.AreEqual (new TextPosition (1, 0), engine.CurrentPosition);

			// Middle of a line.
			engine.SetCursorPosition (new TextPosition (2, 3), true);
			engine.PerformBackspace ();

			Assert.AreEqual ("ba\u0304\u0308", engine.Lines[2]);
			Assert.AreEqual (new TextPosition (2, 1), engine.CurrentPosition);
		}

		[Test]
		public void PerformLeftRight ()
		{
			var engine = new TextEngine (testSnippet);

			engine.SetCursorPosition (new TextPosition (0, 3), true);
			engine.PerformRight (false, false);
			Assert.AreEqual (new TextPosition (0, 4), engine.CurrentPosition);
			engine.PerformRight (false, false);
			Assert.AreEqual (new TextPosition (0, 6), engine.CurrentPosition);
			engine.PerformRight (false, false);
			engine.PerformRight (false, false);
			Assert.AreEqual (new TextPosition (1, 2), engine.CurrentPosition);

			engine.PerformLeft (false, false);
			Assert.AreEqual (new TextPosition (1, 0), engine.CurrentPosition);
			engine.PerformLeft (false, false);
			Assert.AreEqual (new TextPosition (0, 6), engine.CurrentPosition);

			// Should stay at the beginning / end when attempting to advance further.
			engine.SetCursorPosition (new TextPosition (0, 0), true);
			engine.PerformLeft (false, false);
			Assert.AreEqual (new TextPosition (0, 0), engine.CurrentPosition);

			var endPosition = new TextPosition (testSnippet.Count - 1, testSnippet.Last ().Length);
			engine.SetCursorPosition (endPosition, true);
			engine.PerformRight (false, false);
			Assert.AreEqual (endPosition, engine.CurrentPosition);
		}
	}
}
