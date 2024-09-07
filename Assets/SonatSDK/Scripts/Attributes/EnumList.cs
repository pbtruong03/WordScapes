using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class EnumList
{
    public readonly string TypeName;
    public IList<GUIContent> Names;
    public int[] Values;
    public bool Found;

 

    public bool IsNotValid() => Values.HasDuplicate();
    
    public GUIContent GetName(int value)
    {
        if (Values == null)
            return new GUIContent("ErrNotFound_" + TypeName);
        for (var i = 0; i < Values.Length; i++)
        {
            if (Values[i] == value)
                return Names[i];
        }

        return new GUIContent("ErrNotFound");
    }

    public string GetNameString(int value) => GetName(value).text;
    
    public string GetRawName(int value)
    {
        if (Values == null)
            return  ("ErrNotFound_" + TypeName);
        for (var i = 0; i < Values.Length; i++)
        {
            if (Values[i] == value)
                return Names[i].text;
        }

        return ("ErrNotFound");
    }
    
    public EnumList(Type enumType)
    {
        TypeName = enumType.Name;
        Type type = enumType;

        if (type.IsEnum)
        {
            Found = true;
            Names = Enum.GetNames(type).Select(x => new GUIContent(x)).ToList();
            Values = (int[]) Enum.GetValues(type);
        }
#if UNITY_EDITOR
        
#endif
    }
 
    public EnumList(string enumTypeName)
    {
        TypeName = enumTypeName;
        Type type = Type.GetType(enumTypeName);

        if (type != null && type.IsEnum)
        {
            Found = true;
            Names = Enum.GetNames(type).Select(x => new GUIContent(x)).ToList();
            Values = (int[]) Enum.GetValues(type);
        }
#if UNITY_EDITOR
        else
        {
            var projectSettings = ProjectSettings.LoadProjectSettings();
            type = Type.GetType(projectSettings.nameSpace + "." + enumTypeName);
            if (type == null)
                type = GetType(projectSettings.nameSpace, enumTypeName);

            if (type != null && type.IsEnum)
            {
                Found = true;
                Names =  Enum.GetNames(type).Select(x => new GUIContent(x)).ToList();
                Values = ((int[]) Enum.GetValues(type));
            }
        }

        Type GetType(string nameSpace, string className)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type2 = Type.GetType($"{nameSpace}.{className},{assembly.GetName().Name}");
                if (type2 != null)
                    return type2;
            }

            foreach (var assembly in assemblies)
            {
                var type2 = Type.GetType($"{className},{assembly.GetName().Name}");
                if (type2 != null)
                    return type2;
            }

            return null;
        }
#endif
    }

    public EnumList(string prefix,int take)
    {
        Found = true;
        Names =  Enumerable.Range(0,take).Select(x => new GUIContent(prefix+x)).ToList();
        Values = Enumerable.Range(0, take).ToArray();
    }

    
    public void Reload(Type type)
    {
        if (type != null)
        {
            Found = true;
            Names =  Enum.GetNames(type).Select(x => new GUIContent(x)).ToList();
            Values = ((int[]) Enum.GetValues(type));
        }
    }

    public static GUIContent GetName(string enumTypeName, int value)
    {
        var list = new EnumList(enumTypeName);
        if (list.Values.Contains(value))
            return list.Names[list.Values.ToList().IndexOf(value)];
        return new GUIContent("not found");
    }
}

