using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Support.V7.Preferences;
using Android.Widget;

namespace TransmissionRemote.Droid.Preferences
{
    [Activity(Label = "QrcodeGeneratorActivity")]
    public class QrcodeGeneratorActivity: Activity
    {
        ImageView imageBarcode;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var stringToConvert = Intent.GetStringExtra("MyData");
            if (string.IsNullOrEmpty(stringToConvert))
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.qrcode_generation_error), ToastLength.Short).Show();
                Finish();
            }

            SetContentView(Resource.Layout.qr_code_image_view);
            var barcodeWriter = new ZXing.Mobile.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 1000,
                    Height = 1000
                }
            };

            imageBarcode = FindViewById<ImageView>(Resource.Id.image_barcode);
            imageBarcode.SetImageBitmap(barcodeWriter.Write(stringToConvert));
        }
    }
}

