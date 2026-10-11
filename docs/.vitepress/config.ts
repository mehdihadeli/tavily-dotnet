import { defineConfig } from "vitepress";

export default defineConfig({
  title: "Tavily .NET SDK",
  description:
    "Typed .NET access to Tavily search, extraction, crawling, mapping, and research APIs.",
  base: process.env.GITHUB_ACTIONS
    ? `/${process.env.GITHUB_REPOSITORY?.split("/")[1] ?? "tavily-dotnet"}/`
    : "/",
  cleanUrls: true,
  appearance: true,
  lastUpdated: true,
  themeConfig: {
    nav: [
      { text: "Guide", link: "/guide/quickstart" },
      { text: "API capabilities", link: "/guide/capabilities" },
      { text: "Reference", link: "/reference/errors" },
      {
        text: "GitHub",
        link: "https://github.com/mehdihadeli/tavily-dotnet",
      },
    ],
    sidebar: {
      "/guide/": [
        { text: "Versioning and releases", link: "/guide/versioning" },
        {
          text: "Get started",
          items: [
            { text: "Quickstart", link: "/guide/quickstart" },
            { text: "Authentication", link: "/guide/authentication" },
            { text: "API capabilities", link: "/guide/capabilities" },
            { text: "AI tool wrappers", link: "/guide/ai-tools" },
          ],
        },
        {
          text: "Project workflow",
          items: [
            {
              text: "HTTP clients and cancellation",
              link: "/guide/http-client",
            },
            { text: "Build and test", link: "/guide/development" },
          ],
        },
      ],
      "/reference/": [
        {
          text: "Reference",
          items: [
            { text: "Errors", link: "/reference/errors" },
            { text: "Models and operations", link: "/reference/models" },
          ],
        },
      ],
    },
    outline: "deep",
    outlineTitle: "On this page",
    sidebarMenuLabel: "Menu",
    returnToTopLabel: "Return to top",
    darkModeSwitchLabel: "Appearance",
    lightModeSwitchTitle: "Switch to light theme",
    darkModeSwitchTitle: "Switch to dark theme",
    search: { provider: "local" },
    footer: {
      message: "Typed Tavily API access for .NET applications.",
      copyright: "MIT License",
    },
  },
});
