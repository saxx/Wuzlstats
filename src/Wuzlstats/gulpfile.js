/// <binding BeforeBuild='sass' AfterBuild='copy' Clean='clean' />
var gulp = require("gulp"),
  sass = require("gulp-sass"),
  rimraf = require("rimraf"),
  fs = require("fs"),
  uglify = require('gulp-uglifyjs'),
  watch = require('gulp-watch');

eval("var project = " + fs.readFileSync("./project.json"));

var paths = {
    bower: "./bower_components/",
    lib: "./" + project.webroot + "/lib/"
};

gulp.task("clean", function (cb) {
    rimraf(paths.lib, cb);
});

gulp.task('sass', function () {
    gulp.src('./wwwroot/css/*.scss')
      .pipe(sass().on('error', sass.logError))
      .pipe(gulp.dest('./wwwroot/css'));
});

gulp.task('uglify', function () {
    gulp.src('./wwwroot/js/*.js')
        .pipe(uglify('app.js', {
            outSourceMap: true
        }))
        .pipe(gulp.dest('./wwwroot/js/minified'));
});

gulp.task('watch', function () {
    gulp.watch(['./wwwroot/css/*.scss', './wwwroot/js/*.js'], ['sass', 'uglify']);
});

gulp.task("copy", ["clean", "uglify"], function () {
    var bower = {
        "bootstrap": "bootstrap/dist/**/*.{js,map,css,ttf,svg,woff,eot}",
        "jquery": "jquery/dist/jquery*.{js,map}",
        "signalr": "signalr/*.{js,map}",
        "hammerjs": "hammerjs/*.{js,map}"
    }

    for (var destinationDir in bower) {
        if (bower.hasOwnProperty(destinationDir)) {
            gulp.src(paths.bower + bower[destinationDir])
                .pipe(gulp.dest(paths.lib + destinationDir));
        }
    }
});
