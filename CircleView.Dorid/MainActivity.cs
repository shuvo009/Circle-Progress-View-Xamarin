using System;
using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using AT.Grabner.Circleprogress;
using Java.Util;
using Exception = Java.Lang.Exception;

namespace CircleView.Dorid
{
    [Activity(Label = "CircleView.Dorid", MainLauncher = true)]
    public class MainActivity : Activity, CircleProgressView.IOnProgressChangedListener,
        IAnimationStateChangedListener, CompoundButton.IOnCheckedChangeListener
    {
        private static string TAG = "MainActivity";

        CircleProgressView mCircleView;
        Switch mSwitchSpin;
        Switch mSwitchShowUnit;
        SeekBar mSeekBar;
        SeekBar mSeekBarSpinnerLength;
        bool mShowUnit = true;
        Spinner mSpinner;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            mCircleView = FindViewById<CircleProgressView>(Resource.Id.circleView);


            mCircleView.SetOnProgressChangedListener(this);
            mCircleView.ShowTextWhileSpinning = true; // Show/hide text in spinning mode
            mCircleView.SetText("Loading...");
            mCircleView.SetOnAnimationStateChangedListener(this);
            mSwitchSpin = FindViewById<Switch>(Resource.Id.switch1);

            mSwitchSpin.SetOnCheckedChangeListener(this);


            mSwitchShowUnit = FindViewById<Switch>(Resource.Id.switch2);
            mSwitchShowUnit.Checked = mShowUnit;
            mSwitchShowUnit.CheckedChange += (s, e) =>
            {
                mCircleView.UnitVisible = e.IsChecked;
                mShowUnit = e.IsChecked;
            };

            //Setup SeekBar
            mSeekBar = FindViewById<SeekBar>(Resource.Id.seekBar);

            mSeekBar.Max = 100;
            mSeekBar.StopTrackingTouch += (s, e) =>
            {
                mCircleView.SetValueAnimated(mSeekBar.Progress, 1500);
                mSwitchSpin.Checked = false;
            };
            mSeekBarSpinnerLength = FindViewById<SeekBar>(Resource.Id.seekBar2);
            mSeekBarSpinnerLength.Max = 360;
            mSeekBarSpinnerLength.StopTrackingTouch += (s, e) =>
            {
                mCircleView.SetSpinningBarLength(mSeekBarSpinnerLength.Progress);
            };

            mSpinner = FindViewById<Spinner>(Resource.Id.spinner);
            var list = new List<String>();
            list.Add("Left Top");
            list.Add("Left Bottom");
            list.Add("Right Top");
            list.Add("Right Bottom");
            list.Add("Top");
            list.Add("Bottom");
            ArrayAdapter<String> dataAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, 6, list);
            mSpinner.Adapter = dataAdapter;
            mSpinner.ItemSelected += (s, e) =>
            {
                switch (e.Position)
                {
                    case 0:
                        mCircleView.SetUnitPosition(UnitPosition.LeftTop);
                        break;
                    case 1:
                        mCircleView.SetUnitPosition(UnitPosition.LeftBottom);
                        break;
                    case 2:
                        mCircleView.SetUnitPosition(UnitPosition.RightTop);
                        break;
                    case 3:
                        mCircleView.SetUnitPosition(UnitPosition.RightBottom);
                        break;
                    case 4:
                        mCircleView.SetUnitPosition(UnitPosition.Top);
                        break;
                    case 5:
                        mCircleView.SetUnitPosition(UnitPosition.Bottom);
                        break;

                }
            };
            mSpinner.SetSelection(2);

        }

        public void OnProgressChanged(float p0)
        {
            Log.Debug(TAG, "Progress Changed: " + p0);
        }

        public void OnAnimationStateChanged(AnimationState p0)
        {
            switch (p0)
            {
                //case  :
                //case AnimationState.Animating:
                //case AnimationState.StartAnimatingAfterSpinning:
                //    mCircleView.SetTextMode(TextMode.Percent); // show percent if not spinning
                //    mCircleView.UnitVisible = mShowUnit;
                //    break;
                //case AnimationState.Spinning:
                //    mCircleView.SetTextMode(TextMode.Text); // show text while spinning
                //    mCircleView.UnitVisible = false;
                //    break;
                //case AnimationState.EndSpinning:
                //    break;
                //case AnimationState.EndSpinningStartAnimating:
                //    break;

            }
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            if (isChecked)
            {
                mCircleView.Spin();
            }
            else
            {
                mCircleView.StopSpinning();
            }
        }
    }
}

