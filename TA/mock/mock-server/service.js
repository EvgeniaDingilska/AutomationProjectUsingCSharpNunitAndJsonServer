const jsonServer = require('json-server')
const server = jsonServer.create()
const router = jsonServer.router('./db.json')
const port = 1234;
const routes = require('./routes.json')
const middleware = jsonServer.defaults()
router.db._.id = 'id';

server.use(middleware)
server.use(jsonServer.bodyParser)

server.use(function (req, res, next) {
    if (req.method === 'POST') {
        // Converts POST to GET and move payload to query params
        // This way it will make JSON Server that it's GET request
        req.method = 'GET'
        req.query = req.body
    }
    // Continue to JSON Server router
    next()
})

server.use(jsonServer.rewriter(routes))
server.use(router)
server.listen(port, () => {
    console.log('Mock JSON Server is running', port)
})
