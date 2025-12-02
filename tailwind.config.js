module.exports = {
  content: [
    "./src/Wuzlstats/Views/**/*.cshtml",
    "./src/Wuzlstats/wwwroot/js/**/*.js",
    "./node_modules/flowbite/**/*.js"
  ],
  theme: {
    extend: {
      colors: {
        'team-blue': '#ddf',
        'team-red': '#fdd',
      },
    },
  },
  plugins: [
    require('flowbite/plugin')
  ],
}
