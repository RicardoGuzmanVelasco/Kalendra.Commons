using System;
using Kalendra.Commons.Editor;
using NUnit.Framework;
using UnityEngine;

namespace Kalendra.Commons.Tests.Editor
{
    public class LayoutPackageFolderTests
    {
        [Test]
        public void METHOD()
        {
            var sut = PackageLayout.Templates.UnityPackageLayout("Kalendra.TestFolder");
            Debug.Log("\n\n" + sut);
        }
    }
}