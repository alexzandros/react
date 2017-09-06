#load "tools/includes.fsx"

open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.React")
        .VersionFrom("WebSharper")
        .WithFSharpVersion(FSharpVersion.FSharp30)
        .WithFramework(fun x -> x.Net40)

let extension =
    bt.WebSharper4.Extension("WebSharper.React.Bindings")
        .SourcesFromProject()

let wrapper =
    bt.WebSharper4.Library("WebSharper.React")
        .SourcesFromProject()
        .References(fun ref ->
            [
                ref.Project extension
            ])

let tests =
    bt.WebSharper4.BundleWebsite("WebSharper.React.Tests")
        .SourcesFromProject()
        .References(fun ref ->
            [
                ref.Project extension
                ref.Project wrapper
            ])

bt.Solution [
    extension
    wrapper
    tests

    bt.NuGet.CreatePackage()
        .Configure(fun configuration ->
            { configuration with
                Title = Some "WebSharper.React"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://bitbucket.org/intellifactory/websharper.react"
                Description = "WebSharper bindings and abstractions for React."
                Authors = [ "IntelliFactory" ]
                RequiresLicenseAcceptance = true })
        .Add(extension)
        .Add(wrapper)
]
|> bt.Dispatch
