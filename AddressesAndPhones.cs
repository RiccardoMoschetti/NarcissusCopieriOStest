using System;
using System.Collections.Generic;

namespace Narcissus
{

	public class Nation
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class PersonalAddress
	{
		public int Id { get; set; }
		public string Street { get; set; }
		public string ExtraPersonalAddressInfo { get; set; }
		public Nation Nation { get; set; }
	}

	public class OfficeAddress
	{
		public int Id { get; set; }
		public string Street { get; set; }
		public string ExtraOfficeAddressInfo { get; set; }
		public Nation Nation { get; set; }
	}

	public class PersonalPhoneSet
	{
		public int Id { get; set; }
		public string Landline { get; set; }
		public string Mobile { get; set; }
		public string Skype { get; set; }
		public string IntercomNumber { get; set; }
		public List<string> OtherPhones { get; set; }
	}

	public class OfficePhoneSet
	{
		public int Id { get; set; }
		public string Landline { get; set; }
		public string Mobile { get; set; }
		public string SkypeForBusiness { get; set; }
		public List<string> OtherPhones { get; set; }
	}


	public static class AddressFactory
	{
		public static Random _random;

		public static List<PersonalAddress> CreatePersonalAddresses(int instances)
		{
			if (_random == null)
				_random = new Random();
			var personalAddresses = new List<PersonalAddress>();
			for (int i = 0; i < instances; i++)
			{
				personalAddresses.Add
					   (new
						 PersonalAddress
					   {
						   Id = _random.Next(instances),
						   Street = "Street " + _random.Next(instances).ToString(),
						   ExtraPersonalAddressInfo = "ExtraInfo " + _random.Next(instances).ToString(),
						   Nation = new Nation { Id = _random.Next(0, 100), Name = "Nation " + _random.Next(100).ToString() }
					   });
			}
			return personalAddresses;
		}


		public static List<OfficeAddress> CreateOfficeAddresses(int instances)
		{
			if (_random == null)
				_random = new Random();
			var officeAddresses = new List<OfficeAddress>();
			for (int i = 0; i < instances; i++)
			{
				officeAddresses.Add
					   (new
						OfficeAddress()
					   );
			}
			return officeAddresses;
		}

		public static List<PersonalPhoneSet> CreatePersonalPhones(int instances)
		{
			if (_random == null)
				_random = new Random();
			var personalPhones = new List<PersonalPhoneSet>();
			for (int i = 0; i < instances; i++)
			{
				personalPhones.Add
					   (new
						PersonalPhoneSet
					   {
						   Id = _random.Next(instances),
						   IntercomNumber = _random.Next(100).ToString(),
						   Landline = _random.Next(instances).ToString(),
						   Mobile = _random.Next(instances).ToString(),
						   Skype = "SK" + _random.Next(instances),
						   OtherPhones =
						new List<string> {
							_random.Next(instances).ToString(),
							_random.Next(instances).ToString(),
							_random.Next(instances).ToString()}
						 });
			}
			return personalPhones;
		}

		public static List<OfficePhoneSet> CreateOfficePhones(int instances)
		{
			if (_random == null)
				_random = new Random();
			var officePhones = new List<OfficePhoneSet>();
			for (int i = 0; i < instances; i++)
			{
				officePhones.Add
					   (new
						OfficePhoneSet
					   {
						   Id = _random.Next(instances),
						   Landline = _random.Next(instances).ToString(),
						   Mobile = _random.Next(instances).ToString(),
						   SkypeForBusiness = "SKB" + _random.Next(instances),
						   OtherPhones=	new List<string> {
							_random.Next(instances).ToString(),
							_random.Next(instances).ToString(),
							_random.Next(instances).ToString()}
					   });
			}
			return officePhones;
		}

	}

}
