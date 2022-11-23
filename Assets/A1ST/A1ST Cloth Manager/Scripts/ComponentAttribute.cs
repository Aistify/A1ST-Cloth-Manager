#if UNITY_EDITOR

using System;

[AttributeUsage(AttributeTargets.Class)]
public class ComponentAttribute : Attribute
{
    // Init Strings to be gettable only
    public string Name { get; }
    public string Description { get; }

    // Class Object with only Name
    public ComponentAttribute(string name)
    {
        Name = name;
    }

    // Class Object with both Name and a Description
    public ComponentAttribute(string name, string description) : this(name) // What?
    {
        Description = description;
    }
}

#endif
