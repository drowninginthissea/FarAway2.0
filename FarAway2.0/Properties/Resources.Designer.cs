﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FarAway2._0.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FarAway2._0.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы имеете доступ ко манипулированию данными во всех таблицах (все &quot;таблицы-журналы&quot;, включая таблицы со справочной информацией). Также Вам доступен весь функционал отчётности..
        /// </summary>
        internal static string AdminFuncs {
            get {
                return ResourceManager.GetString("AdminFuncs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы имеете доступ (просмотр, редактирование, а также добавление новых данных) ко всей информации, связанной непосредственно с парковочными местами, а также с парковками..
        /// </summary>
        internal static string ControllerFuncs {
            get {
                return ResourceManager.GetString("ControllerFuncs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы имеете полный доступ ко всем &quot;таблицам-журналам&quot;. Вам доступен полный функционал отчётности..
        /// </summary>
        internal static string DirectorFuncs {
            get {
                return ResourceManager.GetString("DirectorFuncs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы имеете доступ (просмотр, редактирование, а также добавление новых данных) ко всей информацией, связанной с пользователями, их арендами, а также дополнительными услугами..
        /// </summary>
        internal static string ManagerFuncs {
            get {
                return ResourceManager.GetString("ManagerFuncs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] RentContract {
            get {
                object obj = ResourceManager.GetObject("RentContract", resourceCulture);
                return ((byte[])(obj));
            }
        }
    }
}
