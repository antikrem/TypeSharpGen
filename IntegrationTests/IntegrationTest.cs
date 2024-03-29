﻿using TypeSharpGen.Specification;
using EphemeralEx.FileSystem;
using EphemeralEx.Extensions;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;
using ReadFile = System.IO.File;
using TypeSharpGenLauncher.Generation;
using TypeSharpGenLauncher.Loading;
using NSubstitute;
using TypeSharpGenLauncher.Core.Constructor;
using TypeSharpGenLauncher.Core.Resolution;
using TypeSharpGenLauncher.Core.Synthesiser;
using TypeSharpGenLauncher.Configuration;
using System;
using NUnit.Framework;
using System.IO;

namespace IntegrationTests
{
    public abstract class IntegrationTest : GenerationSpecification
    {
        private MemoryPersistenceEndpoint _endpoint;

        public override string OutputRoot => $"";

        [SetUp]
        public void SetUp()
        {
            _endpoint = new();
        }

        [Test]
        public void AssertOutput()
        {
            var sut = CreateSut();

            sut.Generate();

            var expected = ToFileSequence(EphemeralEx.FileSystem.Directory.Create($"{TestName}/Expected"));
            var output = _endpoint.Files;

            output.Should().BeEquivalentTo(expected);
        }

        private static IEnumerable<File> ToFileSequence(EphemeralEx.FileSystem.Directory root)
            => root
                .TraverseTree(directory => directory.ChildDirectories)
                .SelectMany(directory => directory.ChildFiles)
                .Select(file => new File(file.Name, ReadFile.ReadAllText(file.Path)));

        private Generation CreateSut()
        {
            var typeReducer = new TypeReducer();
            var typeScriptBuiltInTypes = new TypeScriptBuiltInTypes();
            var projectFolders = Substitute.For<IProjectFolders>();
            projectFolders.ProjectRootPath.Returns("");

            var typesLoader = Substitute.For<ITypesLoader>();
            typesLoader.AllTypes.Returns(GetType().Assembly.Types());

            var generationSpecificationFinder = Substitute.For<IGenerationSpecificationFinder>();
            generationSpecificationFinder
                .FilterSpecifications(Arg.Any<IEnumerable<Type>>())
                .Returns(this.ToEnumerable());

            var typeModelConstructor = new TypeModelConstructor(
                typeScriptBuiltInTypes,
                typeReducer,
                Substitute.For<IAssemblyLoader>()
            );

            var modelResolver = new ModelResolver(typeReducer, typeScriptBuiltInTypes);

            var synthesiser = new Synthesiser(
                new DeclarationFileSynthesiser(projectFolders, _endpoint.ToEnumerable())
            );

            return new Generation(typesLoader, generationSpecificationFinder, typeModelConstructor, modelResolver, synthesiser);
        }

        private string TestName => GetType().Name;

        private record File(string Name, string Content);


        private class MemoryPersistenceEndpoint : IEmisionEndpoint
        {
            private List<File> _files = new();

            public void Write(string location, string body)
            {
                _files.Add(new File(location[1..], body));
            }

            public IEnumerable<File> Files => _files;
        }
    }
}