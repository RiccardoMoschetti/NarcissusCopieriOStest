using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Narcissus
{
	public struct ConversionPair
	{
		public Type TSource { get; set; }
		public Type TDestination { get; set; }
	}

	public static class NarcissusCopier<TSource, TDestination>
	{
		
		// This method allows you to copy an object on another object, based on the common properties they have.
		// Syntax:
		// NarcissusCopier<TypeOfSourceObject, TypeOfDestinationObject>.CopyAnyObject(SourceObjectInstance, DestinationObjectInstance);
		// To improve performance in case the copies of two objects are executed more than once, 
		// the method pair RegisterObjectProperties and CopyRegisteredObject is more indicated
		public static void CopyAnyObject(TSource source, TDestination destination)
		{
			var propertiesOfSource = source.GetType().GetProperties();
			var propertiesOfDestination = destination.GetType().GetProperties();
			var propertiesInCommon =
				(from a in propertiesOfSource
				 join b in propertiesOfDestination
				 on new { a.Name, a.PropertyType } equals new { b.Name, b.PropertyType }
				 select a);
			foreach (var propertyInCommon in propertiesInCommon)
			{
				var valueOfPropertyToCopy = propertyInCommon.GetValue(source, null);
				PropertyInfo propertyOfDestinationToReplace = destination.GetType().GetProperty(propertyInCommon.Name);	
				propertyOfDestinationToReplace.SetValue(destination,
											   Convert.ChangeType(valueOfPropertyToCopy,
																  propertyOfDestinationToReplace.PropertyType), null);
			}
		}

		// This is the array where the properties in common between two classes are stored
		// The compiler warns that this is a static field in a generic type, which is however exactly what we need in this case:
		// since the field for the classes typed with <TX, TY> is different that when the class is typed with <TW,TZ>, 
		// we don't need to store class / property correspondencies in a dictionary
		static PropertyInfo[] _registeredPropertiesInCommon;

		// This method stores the common properties between two objects, so that copy is faster than with CopyAnyObject
		// To call this method, use NarcissusCopier<TypeOfSourceObject, TypeOfDestinationObject>.RegisterObjectProperties();
		public static void RegisterTwoObjectCommonProperties()
		{

			var propertiesOfSource = typeof(TSource).GetProperties();
			var propertiesOfDestination = typeof(TDestination).GetProperties();
			var propertiesInCommon =
				(from a in propertiesOfSource
				 join b in propertiesOfDestination
				 on new { a.Name, a.PropertyType } equals new { b.Name, b.PropertyType }
				 select a);
			_registeredPropertiesInCommon = propertiesInCommon.ToArray();
		}

		// This method copies the content of an object on another, based on the common property names and types that have been stored
		// via the RegisterObjectProperties
		// To call this method and copy an object on another, you use
		// NarcissusCopier<TypeOfSourceObject, TypeOfDestinationObject>.CopyRegisteredObject(SourceObjectInstance, DestinationObjectInstance);
		// However, before being able to do this, you have to register the pair with the call 
		// NarcissusCopier<TypeOfSourceObject, TypeOfDestinationObject>.RegisterObjectProperties();
		public static void CopyRegisteredObject(TSource source, TDestination destination)
		{
			foreach (var propertyInCommon in _registeredPropertiesInCommon)
			{
				var valueOfPropertyToCopy = propertyInCommon.GetValue(source, null);
				PropertyInfo propertyOfDestinationToReplace = destination.GetType().GetProperty(propertyInCommon.Name);
				propertyOfDestinationToReplace.SetValue(destination,
											   Convert.ChangeType(valueOfPropertyToCopy,
																  propertyOfDestinationToReplace.PropertyType), null);
			}
		}

	}

}
