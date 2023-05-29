using Comparator.DTOs;
using Comparator.Models;
using System.Reflection;

namespace Comparator.Services;
public class PropsComperator
{
    public Offer UpdateViaReflection(Offer offer, UpdatableOfferDTO newParams)
    {
        Type DTOParams = newParams.GetType();
        PropertyInfo[] DTOContactAMLDataProps = DTOParams.GetProperties();

        Type offerForUpdate = offer.GetType();
        PropertyInfo[] contactAMlDataForUpdateProps = offerForUpdate.GetProperties();

        foreach (var pDTO in DTOContactAMLDataProps)
        {
            foreach (var pAML in contactAMlDataForUpdateProps)
            {
                if (pDTO.Name == pAML.Name)
                {
                    PropertyInfo tempPropAML = contactAMlDataForUpdateProps.FirstOrDefault(p => p.Name == pAML.Name);
                    PropertyInfo tempPropDTO = DTOContactAMLDataProps.FirstOrDefault(p => p.Name == pDTO.Name);
                    Type tempPropTypeDTO = tempPropDTO.PropertyType;

                    object defaultValue = GetDefaultValue(tempPropDTO.PropertyType);
                    object propertyValue = tempPropDTO.GetValue(DTOParams);

                    if (defaultValue == null && propertyValue == null)
                    {
                        continue;
                    }

                    bool hasDefaultValue = propertyValue.Equals(defaultValue);
                    if (!hasDefaultValue)
                    {
                        if (tempPropTypeDTO.IsGenericType && tempPropTypeDTO.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            Type nullabulleDataType = Nullable.GetUnderlyingType(tempPropTypeDTO);
                            tempPropAML.SetValue(offerForUpdate, Convert.ChangeType(propertyValue, nullabulleDataType));

                            continue;
                        }

                        tempPropAML.SetValue(offerForUpdate, Convert.ChangeType(propertyValue, tempPropTypeDTO));
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        return offer;
    }
    public static object GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}
    
