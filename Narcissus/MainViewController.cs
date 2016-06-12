using System;
using System.Collections.Generic;
using System.Diagnostics;
using UIKit;
using CoreGraphics;

namespace Narcissus
{
	public partial class MainViewController : UIViewController
	{
		
		public MainViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public MainViewController() 
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		UIScrollView MyScrollView;
		nint scrollPos;
		List<PersonalAddress> _personalAddresses;
		List<OfficeAddress> _officeAddresses;
		List<PersonalPhoneSet> _personalPhones;
		List<OfficePhoneSet> _officePhones;


		public void RefreshAddressesAndPhones(int instances)
		{ 
		    _personalAddresses = AddressFactory.CreatePersonalAddresses(instances);
			_officeAddresses = AddressFactory.CreateOfficeAddresses(instances);
			_personalPhones = AddressFactory.CreatePersonalPhones(instances);
			_officePhones = AddressFactory.CreateOfficePhones(instances);
		}

		public override async void ViewDidLoad()
		{
			
			const int instances = 200000;
			scrollPos = 60;

			base.ViewDidLoad();
			this.View.BackgroundColor = UIColor.LightGray;
			CreateMyScrollView();

			AddLabel("COPY COMPARISONS", 45);

			AddLabel($"{instances} copies", 45);
			var worker = new Worker();

			AddLabel("Direct copy", 25);
			RefreshAddressesAndPhones(instances);
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			await worker.CopyDataWithoutReflection(_personalAddresses, _officeAddresses, _personalPhones, _officePhones);
			stopwatch.Stop();
			var directCopyTime = stopwatch.ElapsedMilliseconds;
			var success =
				worker.CheckAddressCopy(_personalAddresses, _officeAddresses)
				&& worker.CheckPhoneCopy(_personalPhones, _officePhones);
			AddLabel("Result of copy: " + (success ? "OK" : "KO"), 25);
			AddLabel("Direct copy time in ms: " + directCopyTime, 45);

			AddLabel("Non-prepared reflection copy", 25);
			RefreshAddressesAndPhones(instances);
			stopwatch.Reset();
			stopwatch.Start();
			await worker.CopyDataWithNonPreparedReflection(_personalAddresses, _officeAddresses, _personalPhones, _officePhones);
			stopwatch.Stop();
			var nonPreparedReflectionCopyTime = stopwatch.ElapsedMilliseconds;
			AddLabel("Result of copy: " + (success ? "OK" : "KO"), 25);
			AddLabel("Non-prepared copy time in ms: " + nonPreparedReflectionCopyTime, 45);


			AddLabel("Prepared reflection copy", 25);
			RefreshAddressesAndPhones(instances);
			stopwatch.Reset();
			stopwatch.Start();
			await worker.CopyAddressesWithPreparedReflection(_personalAddresses, _officeAddresses, _personalPhones, _officePhones);
			stopwatch.Stop();
			var preparedReflectionCopyTime = stopwatch.ElapsedMilliseconds;
			success = worker.CheckAddressCopy(_personalAddresses, _officeAddresses)
				&& worker.CheckPhoneCopy(_personalPhones, _officePhones);
			AddLabel("Result of copy: " + (success ? "OK" : "KO"), 25);
			AddLabel("Prepared copy time in ms: " + preparedReflectionCopyTime, 45);

		}

		void CreateMyScrollView()
		{
			MyScrollView = new UIScrollView(new CGRect(0, 20, 600, 500));
			MyScrollView.ContentSize = new CoreGraphics.CGSize(600, 2000);
			MyScrollView.ScrollEnabled = true;
			View.AddSubview(MyScrollView);
		}

		public void AddLabel(string text, nint offset)
		{
			UILabel label = new UILabel(new CGRect(20, scrollPos, 400, 25));
			label.Text = text;
			label.Font.WithSize(16);
			MyScrollView.AddSubview(label);
			scrollPos += offset;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

