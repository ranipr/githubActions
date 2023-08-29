using System;

namespace GiveawayHistorianScheduler.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredAttribute : Attribute
    {
        public RequiredAttribute()
        {
        }
    }

    /// <summary>
    /// Tag Attribute will map Machine Models with their Tags.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TagAttribute : Attribute
    {
        public string TagName { get; set; }

        public TagAttribute(string tagName)
        {
            this.TagName = tagName;
        }
    }

    /// <summary>
    /// Attribute when defined will aggregate all properties according to the TS.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AggregateAttribute : Attribute
    {
        public AggregateAttribute()
        {
        }
    }

    /// <summary>
    /// The Cache Key Prefix to be added while storing a model to the redis cache.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CacheKeyPrefixAttribute : Attribute
    {
        public string KeyPrefix { get; set; }

        public CacheKeyPrefixAttribute(string keyPrefix)
        {
            this.KeyPrefix = keyPrefix;
        }
    }

    /// <summary>
    /// The Machine Data Type to correctly identify the type of data expected.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MachineDataTypeAttribute : Attribute
    {
        public string MachineDataType { get; set; }

        public MachineDataTypeAttribute(string machineDataType)
        {
            this.MachineDataType = machineDataType;
        }
    }


    /// <summary>
    /// Define the KubeArg for Configurations so that they are correctly mapped with the arguments passed in the Deployment yaml of Kubernetes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class KubeArgAttribute : Attribute
    {
        public string KubeArgName { get; set; }

        public KubeArgAttribute(string kubeArgName)
        {
            this.KubeArgName = kubeArgName;
        }
    }

    /// <summary>
    /// Attribute used for printing Sensitive Arguments to Console.
    /// If reveal length is set to 0 then entire argument is hashed.
    /// Reveal Length defined >0 will reveal that many characters to the console's writeline.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SensitiveAttribute : Attribute
    {
        public int revealLength { get; set; }

        public SensitiveAttribute(int revealLength)
        {
            this.revealLength = revealLength;
        }
    }

    /// <summary>
    /// Tag Attribute will map Machine Models with their Tags.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TwinPropertyAttribute : Attribute
    {
        public string TwinPropertyName { get; set; }

        public TwinPropertyAttribute(string twinPropertyName)
        {
            this.TwinPropertyName = twinPropertyName;
        }
    }
}