using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Widget;
using Android.Content;

namespace AndroidDragNDrop
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnTouchListener, View.IOnDragListener
    {
		private TextView txtQ;
		private TextView txtA;
		public string[] LeftOptions = { "sh. hosts", "sh. interface s 0", "ping" };
        public string[] RightOptions = { "Displays the host name(s) and related IP address(es)", "sh. interface s 0Enables you to look at the encapsulation type", "Sends an ICMP echo message" };
        int pix = 16;
        int bwidth = 0;
        int bheight = 0;
        int bwidth1 = 0;
        int bheight1 = 0;
        float scale = 0;
        ScrollView scrollv;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.textdragndrop);

            

            Button cmdReset = FindViewById<Button>(Resource.Id.Reset);
            cmdReset.Click += new EventHandler(cmdReset_Click);

            Button cmdShow = FindViewById<Button>(Resource.Id.Show);
            cmdShow.Click += new EventHandler(cmdShow_Click);

            bwidth1 = (int)this.Resources.GetInteger((Resource.Integer.boxwidth));

            scale = this.Resources.DisplayMetrics.Density;
            bwidth = (int)(bwidth1 * scale);

            bheight1 = (int)this.Resources.GetInteger((Resource.Integer.boxheight));
            bheight = (int)(bheight1 * scale);

            scrollv = FindViewById<ScrollView>(Resource.Id.scroll1);
            GenerateQuestions();
        }

        void cmdShow_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                string txtans = LeftOptions[i];
               // int tempCorAns = GetCorrectDragAns1(qid, i);
                TextView tempTxtA = FindViewById<TextView>(i + 10);
                tempTxtA.Text = txtans;
                tempTxtA.SetTextSize(Android.Util.ComplexUnitType.Sp, pix);
                tempTxtA.SetBackgroundColor(Android.Graphics.Color.LightBlue);
                tempTxtA.Tag = i;
                tempTxtA.SetOnDragListener(this);
            }
        }
        private void GenerateQuestions()
		{
			TableLayout tblMain = FindViewById<TableLayout>(Resource.Id.tableQues);
			for (int i = 0; i < 3; i++)
			{
				TableRow row = new TableRow(this);
				txtQ = new TextView(this);
				txtA = new TextView(this);
				TextView txtSpace = new TextView(this);

				string txtques = LeftOptions[i];
				string txtans = RightOptions[i];
				

				txtSpace.SetWidth(40);
				txtSpace.SetHeight(40);

				txtQ.Text = txtques;
				txtQ.SetPadding(5, 5, 5, 5);
				txtQ.SetWidth(bwidth);
				txtQ.SetHeight(bheight);
				txtQ.Tag = i;
				txtQ.Id = i;
				txtQ.SetTextSize(Android.Util.ComplexUnitType.Sp, pix);
				txtQ.SetBackgroundColor(Android.Graphics.Color.Gray);
                txtQ.SetOnTouchListener(this);


                txtA.Text = txtans;
				txtA.SetPadding(5, 5, 5, 5);
				txtA.Id = i + 10;
				txtA.SetWidth(bwidth);
				txtA.SetHeight(bheight);
				txtA.SetTextSize(Android.Util.ComplexUnitType.Sp, pix);
				txtA.SetBackgroundColor(Android.Graphics.Color.LightGray);
				txtA.SetOnDragListener(this);

                row.SetPadding(5, 5, 5, 5);
                row.AddView(txtQ, new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.WrapContent));
                row.AddView(txtSpace, new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.WrapContent));
                row.AddView(txtA, new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.WrapContent));
                tblMain.AddView(row);
            }		
		}			
		void cmdReset_Click(object sender, EventArgs e)
        {
           
            for (int i = 0; i < 3; i++)
            {             
                string txtans = RightOptions[i];
               
                TextView tempTxtA = FindViewById<TextView>(i + 10);            
                tempTxtA.Text = txtans;
                tempTxtA.SetTextSize(Android.Util.ComplexUnitType.Sp, pix);
                tempTxtA.SetBackgroundColor(Android.Graphics.Color.LightGray);
                tempTxtA.Tag = null;
                tempTxtA.SetOnDragListener(this);
            }          
        }
        void cmdClose_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Drag and Drop results are saved now", ToastLength.Long).Show();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                //Toast.MakeText (this, e.RawY.ToString (), ToastLength.Short).Show ();
                ClipData data = ClipData.NewPlainText("", "");
                View.DragShadowBuilder shdowbuilder = new View.DragShadowBuilder(v);
                v.StartDrag(data, shdowbuilder, v, 0);
                //oldY = e.RawY;

            }
            return true;
        }

        public bool OnDrag(View v, DragEvent e)
        {
            if (e.Action == DragAction.Drop)
            {
                View view = (View)e.LocalState;
                TextView dropTarget = (TextView)v;

                TextView dropped = (TextView)view;
                string txt = dropped.Text.ToString();
                dropTarget.Text = txt;
                dropTarget.SetBackgroundColor(Android.Graphics.Color.LightBlue);
                Object tag = dropTarget.Tag;
                dropTarget.Tag = dropped.Tag;
            }
            else
            {
                if (e.Action == DragAction.Location)
                {
                    int y = (int)Math.Round(e.GetY());
                    int translatedY = y - 10;
                    int threshold = 50;
                    // make a scrolling up due the y has passed the threshold
                    if (translatedY < threshold)
                    {
                        // make a scroll up by 30 px
                        scrollv.ScrollBy(0, -30);
                    }
                    // make a autoscrolling down due y has passed the 500 px border
                    if (translatedY + threshold > 50)
                    {
                        // make a scroll down by 30 px
                        scrollv.ScrollBy(0, 30);
                    }
                }
            }
           
            return true;
        }
    }
}
