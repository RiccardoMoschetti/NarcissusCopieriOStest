using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narcissus
{
	public class Worker
	{
		public async Task CopyDataWithoutReflection
			(List<PersonalAddress> personalAddresses, List<OfficeAddress> officeAddresses,
			 List<PersonalPhoneSet> personalPhones, List<OfficePhoneSet> officePhones)
		{
			await Task.Run(() =>
			{
				for (int i = 0; i < personalAddresses.Count; i++)
				{
					ManualAddressMapper(personalAddresses[i], officeAddresses[i]);
					ManualPhoneMapper(personalPhones[i], officePhones[i]);
				}
			});
		}

		public void ManualAddressMapper(PersonalAddress personalAddress, OfficeAddress officeAddress)
		{
			officeAddress.Id = personalAddress.Id;
			officeAddress.Street = personalAddress.Street;
			officeAddress.Nation = personalAddress.Nation;
		}

		public void ManualPhoneMapper(PersonalPhoneSet personalPhones, OfficePhoneSet officePhones)
		{
			officePhones.Id = personalPhones.Id;
			officePhones.Landline = personalPhones.Landline;
			officePhones.Mobile = personalPhones.Mobile;
			officePhones.OtherPhones = personalPhones.OtherPhones;
		}

		public async Task CopyDataWithNonPreparedReflection
		(List<PersonalAddress> personalAddresses, List<OfficeAddress> officeAddresses,
		 List<PersonalPhoneSet> personalPhones, List<OfficePhoneSet> officePhones)
		{
			await Task.Run(() =>
			{
				for (int i = 0; i < personalAddresses.Count; i++)
				{
					NarcissusCopier<PersonalAddress, OfficeAddress>.CopyAnyObject(personalAddresses[i], officeAddresses[i]);
					NarcissusCopier<PersonalPhoneSet, OfficePhoneSet>.CopyAnyObject(personalPhones[i], officePhones[i]);
				}
			});
		}

		public async Task CopyAddressesWithPreparedReflection
		(List<PersonalAddress> personalAddresses, List<OfficeAddress> officeAddresses,
		 List<PersonalPhoneSet> personalPhones, List<OfficePhoneSet> officePhones)
		{
			await Task.Run(() =>
			{

				NarcissusCopier<PersonalAddress, OfficeAddress>.RegisterTwoObjectCommonProperties();
				NarcissusCopier<PersonalPhoneSet, OfficePhoneSet>.RegisterTwoObjectCommonProperties();
				for (int i = 0; i < personalAddresses.Count; i++)
				{
					NarcissusCopier<PersonalAddress, OfficeAddress>.CopyRegisteredObject(personalAddresses[i], officeAddresses[i]);
					NarcissusCopier<PersonalPhoneSet, OfficePhoneSet>.CopyRegisteredObject(personalPhones[i], officePhones[i]);
				}
			});
		}

		public bool CheckAddressCopy(List<PersonalAddress> personalAddresses, List<OfficeAddress> officeAddresses)
		{
			int testedAddress = new Random().Next(0, personalAddresses.Count);
			return
				(
					personalAddresses[testedAddress].Street == officeAddresses[testedAddress].Street
				);
		}

		public bool CheckPhoneCopy(List<PersonalPhoneSet> personalPhones, List<OfficePhoneSet> officePhones)
		{
			int testedPhone = new Random().Next(0, personalPhones.Count);
			return
				(
					personalPhones[testedPhone].Landline == officePhones[testedPhone].Landline
				);
		}
	}

}
