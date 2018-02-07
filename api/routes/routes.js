
var appRouter = function(app) {
  app.get("/", function(req, res) {
    var pos = Math.floor((Math.random() * 6) + 1);
    if (req.headers.start){
        return res.send("{\"score\":\"0\",\"position\":\"" + pos + "\"}");
    }
    var newScore = Number(req.headers.score) + 1;
    res.send("{\"score\":\"" + newScore + "\",\"position\":\"" + pos + "\"}");
  });
}

module.exports = appRouter;
