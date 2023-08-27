using Comparator.DTOs;
using Comparator.Models;
using System.Reflection;

namespace Comparator.Services;
public class PropsComperator
{
    public Offer UpdateViaReflection(Offer contactAMLDataNew, UpdatableOfferDTO DTOParams)
    {
        Type DTOContactAMLData = DTOParams.GetType();
        PropertyInfo[] DTOContactAMLDataProps = DTOContactAMLData.GetProperties();

        Type contactAMlDataForUpdate = contactAMLDataNew.GetType();
        PropertyInfo[] contactAMlDataForUpdateProps = contactAMlDataForUpdate.GetProperties();

        foreach (var pDTO in DTOContactAMLDataProps)
        {
            foreach (var pAML in contactAMlDataForUpdateProps)
            {
                if (pDTO.Name == pAML.Name)
                {
                    Type tempPropTypeDTO = pDTO.PropertyType;

                    object defaultValue = GetDefaultValue(pDTO.PropertyType);
                    object propertyValue = pDTO.GetValue(DTOParams);

                    if (defaultValue == null && propertyValue == null)
                    {
                        break;
                    }

                    if (propertyValue != null)
                    {
                        bool hasDefaultValue = propertyValue.Equals(defaultValue);

                        if (!hasDefaultValue)
                        {
                            if (tempPropTypeDTO.IsGenericType && tempPropTypeDTO.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                Type nullabulleDataType = Nullable.GetUnderlyingType(tempPropTypeDTO);
                                pAML.SetValue(contactAMLDataNew, Convert.ChangeType(propertyValue, nullabulleDataType));

                                break;
                            }

                            pAML.SetValue(contactAMLDataNew, Convert.ChangeType(propertyValue, tempPropTypeDTO));
                            break;

                        }
                    }
                }
            }
        }
        return contactAMLDataNew;
    }
    public static object GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}
    
