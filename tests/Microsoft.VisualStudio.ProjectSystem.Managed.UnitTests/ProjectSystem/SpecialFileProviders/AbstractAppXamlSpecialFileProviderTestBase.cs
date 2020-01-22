﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;

namespace Microsoft.VisualStudio.ProjectSystem.SpecialFileProviders
{
    public abstract class AbstractAppXamlSpecialFileProviderTestBase : AbstractFindByNameSpecialFileProviderTestBase
    {
        protected AbstractAppXamlSpecialFileProviderTestBase(string fileName)
            : base(fileName)
        {
        }

        [Theory]
        [InlineData(@"
Project (flags: {ProjectRoot}), FilePath: ""C:\Project\Project.csproj""
    Application.xaml, ItemType: ApplicationDefinition, FilePath: ""C:\Project\Application.xaml""
",
@"C:\Project\Application.xaml")]
        [InlineData(@"
Project (flags: {ProjectRoot}), FilePath: ""C:\Project\Project.csproj""
    App.xaml, ItemType: ApplicationDefinition, FilePath: ""C:\Project\App.xaml""
",
@"C:\Project\App.xaml")]
        [InlineData(@"
Project (flags: {ProjectRoot}), FilePath: ""C:\Project\Project.csproj""
    ThisCanBeAnyName.xaml, ItemType: ApplicationDefinition, FilePath: ""C:\Project\ThisCanBeAnyName.xaml""
",
@"C:\Project\ThisCanBeAnyName.xaml")]
        [InlineData(@"
Project (flags: {ProjectRoot}), FilePath: ""C:\Project\Project.csproj""
    Application.xaml, ItemType: ApplicationDefinition, FilePath: ""C:\Project\Application.xaml""
    App.xaml, ItemType: ApplicationDefinition, FilePath: ""C:\Project\App.xaml""
",
@"C:\Project\Application.xaml")]
        [InlineData(@"
Project (flags: {ProjectRoot}), FilePath: ""C:\Project\Project.csproj""
    Properties
        Application.xaml, ItemType: ApplicationDefinition, FilePath: ""C:\Project\Properties\Application.xaml""
",
@"C:\Project\Properties\Application.xaml")]
        [InlineData(@"
Project (flags: {ProjectRoot}), FilePath: ""C:\Project\Project.csproj""
    Properties
        App.xaml, ItemType: ApplicationDefinition, FilePath: ""C:\Project\Properties\App.xaml""
    Application.xaml, ItemType: ApplicationDefinition, FilePath: ""C:\Project\Application.xaml""
",
@"C:\Project\Application.xaml")]        // Breadthfirst
        public async Task GetFileAsync_WhenItemMarkedWithApplicationManifest_ReturnsPath(string input, string expected)
        {
            var tree = ProjectTreeParser.Parse(input);
            var provider = CreateInstance(tree);

            var result = await provider.GetFileAsync(SpecialFiles.AppXaml, SpecialFileFlags.FullPath);

            Assert.Equal(expected, result);
        }
    }
}
