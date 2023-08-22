// Copyright (c) 2015 - 2023 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using System.Collections.Generic;
// ReSharper disable All
namespace Doozy.Runtime.UIManager.Containers
{
    public partial class UIView
    {
        public static IEnumerable<UIView> GetViews(UIViewId.Level id) => GetViews(nameof(UIViewId.Level), id.ToString());
        public static void Show(UIViewId.Level id, bool instant = false) => Show(nameof(UIViewId.Level), id.ToString(), instant);
        public static void Hide(UIViewId.Level id, bool instant = false) => Hide(nameof(UIViewId.Level), id.ToString(), instant);
    }
}

namespace Doozy.Runtime.UIManager
{
    public partial class UIViewId
    {
        public enum Level
        {
            CompleteLevel,
            FailLevel,
            InLevel,
            PreLevel,
            Resource,
            Settings
        }    
    }
}