﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DAL.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to DROP TRIGGER modifiedDate.
        /// </summary>
        internal static string OrderModifiedDateTigger_DOWN {
            get {
                return ResourceManager.GetString("OrderModifiedDateTigger_DOWN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TRIGGER modifiedDate
        ///ON Orders
        ///AFTER INSERT, UPDATE
        ///AS
        ///	UPDATE [Orders] SET ModifiedDate = (SELECT getdate())
        ///	FROM [Orders] x
        ///		INNER JOIN inserted y
        ///		ON x.Id = y.Id
        ///GO.
        /// </summary>
        internal static string OrderModifiedDateTigger_UP {
            get {
                return ResourceManager.GetString("OrderModifiedDateTigger_UP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP PROCEDURE OrderSummary.
        /// </summary>
        internal static string OrderSummary_DOWN {
            get {
                return ResourceManager.GetString("OrderSummary_DOWN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE OR ALTER PROCEDURE OrderSummary
        ///@id int
        ///AS
        ///BEGIN
        ///
        ///	SELECT o.Id, o.DateTime, COUNT(p.Id) AS Count
        ///	FROM [Orders] as o
        ///	JOIN Products as p ON o.Id = p.OrderId
        ///	WHERE o.Id = @id
        ///	GROUP BY o.Id, o.[DateTime]	
        ///
        ///END.
        /// </summary>
        internal static string OrderSummary_UP {
            get {
                return ResourceManager.GetString("OrderSummary_UP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP VIEW View_OrderSummary.
        /// </summary>
        internal static string OrderSummaryView_DOWN {
            get {
                return ResourceManager.GetString("OrderSummaryView_DOWN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE VIEW View_OrderSummary AS
        ///	SELECT o.Id, o.DateTime, COUNT(p.Id) AS Count
        ///	FROM [Orders] as o
        ///	JOIN Products as p ON o.Id = p.OrderId
        ///	GROUP BY o.Id, o.[DateTime]	.
        /// </summary>
        internal static string OrderSummaryView_UP {
            get {
                return ResourceManager.GetString("OrderSummaryView_UP", resourceCulture);
            }
        }
    }
}
