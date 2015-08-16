/// <binding BeforeBuild='sass uglify' />
var gulp = require("gulp"),
  sass = require("gulp-sass"),
  uglify = require('gulp-uglify'),
  concat = require('gulp-concat'),
  watch = require('gulp-watch');

gulp.task('sass', function () {
    gulp.src('./wwwroot/css/*.scss')
      .pipe(sass().on('error', sass.logError))
      .pipe(gulp.dest('./wwwroot/css'));
});

gulp.task('uglify', function () {
    gulp.src('./wwwroot/js/*.js')
        .pipe(concat('app.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest('./wwwroot/js/minified/'));
});

gulp.task('watch', function () {
    gulp.watch(['./wwwroot/css/*.scss', './wwwroot/js/*.js'], ['sass', 'uglify']);
});
