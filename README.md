# silly-maxing-but-in-vs
## Description
Visual Studio Extension kit containing changes I think were fun and worthwhile to adding to VS to improve my productivity through laughs and not wanting to experience the dreaded sound of failure.

## Troubleshooting
Run the following command should something go wrong preventing Visual Studio from starting if the extensions are suspected to be the reason for failure.

``` "[VS Filepath]\Microsoft Visual Studio\[Year (2019/2022/2026)]\[Version (Community/Professional)\Common7\IDE\devenv.exe" /SafeMode ```

This will should start Visual Studio in safe mode which will exclude the loading of all extensions. From here you can open the Extensions menu to uninstall the suspected problematic extensions.