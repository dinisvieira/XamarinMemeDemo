using System;
using System.Collections.ObjectModel;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace XamarinMemeGenerator.Droid
{
	[Activity (Label = "XamarinMemeGenerator.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{

        protected override async void OnCreate (Bundle bundle)
		{
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Reference to all the View Elements
            Button button = FindViewById<Button>(Resource.Id.buttonGenerate);
            Spinner memesSpinner = FindViewById<Spinner>(Resource.Id.spinnerMemes);
            EditText editTextTop = FindViewById<EditText>(Resource.Id.textTop);
            EditText editTextBottom = FindViewById<EditText>(Resource.Id.textBottom);
            ImageView imageViewMeme = FindViewById<ImageView>(Resource.Id.imageMeme);

            //Calls the Shared Portable Class Library to get a list with all available meme's.
            ObservableCollection<string> memes = await Memes.GetMemesList();

            //Set the list of memes to our Spinner and enable it
            var adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, memes);
            memesSpinner.Adapter = adapter;

            button.Click += async delegate
            {
                //Calls the Shared Portable Class Library with the values of the Spinner and TextBox's in this View.
                //The returned value is the image in a byte array format 

                var selMemeName = memesSpinner.SelectedItem.ToString();
                byte[] imageBytes = await Memes.GenerateMeme(selMemeName, editTextTop.Text, editTextBottom.Text);

                //Create Image
                var selectedBmp = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length); 

                //Set image to the Image Placeholder we have on our View
                imageViewMeme.SetImageBitmap(selectedBmp); 
            };
        }
	}
}