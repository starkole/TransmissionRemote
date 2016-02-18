// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TransmissionRemote.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIStackView mainStackView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (mainStackView != null) {
				mainStackView.Dispose ();
				mainStackView = null;
			}
		}
	}
}
