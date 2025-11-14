# Gets rid of all intermediate byproducts and keep the necessary useful stuff
command='git clean -xdf -e .config -e .vscode -e Concepts -e .runsettings'
echo "Running command: '$command' ..."
git clean -xdf -e .config -e .vscode -e Concepts -e .runsettings # -n << dry-run flag

