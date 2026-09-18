const path = require('path');
const express = require('express');
const session = require('express-session');
const authRoutes = require('./routes/auth');

const app = express();
const PORT = process.env.PORT || 3000;

app.use(express.json());
app.use(express.static(path.join(__dirname, '..', 'public')));

app.use(
  session({
    secret: process.env.SESSION_SECRET || require('crypto').randomBytes(32).toString('hex'),
    resave: false,
    saveUninitialized: false,
    cookie: {
      httpOnly: true,
      maxAge: 1000 * 60 * 60 * 24, // 1 día
    },
  })
);

app.use('/api/auth', authRoutes);

function requireAuth(req, res, next) {
  if (!req.session.userId) {
    return res.status(401).json({ error: 'No autenticado.' });
  }
  next();
}

app.get('/api/dashboard', requireAuth, (req, res) => {
  res.json({ message: 'Bienvenido al área privada de DERMA.' });
});

app.listen(PORT, () => {
  console.log(`DERMA corriendo en http://localhost:${PORT}`);
});
