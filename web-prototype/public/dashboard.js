const welcomeMsg = document.getElementById('welcomeMsg');

async function loadUser() {
  const res = await fetch('/api/auth/me');
  if (!res.ok) {
    window.location.href = '/index.html';
    return;
  }
  const user = await res.json();
  welcomeMsg.textContent = `Hola, ${user.name} (${user.email})`;
}

document.getElementById('logoutBtn').addEventListener('click', async () => {
  await fetch('/api/auth/logout', { method: 'POST' });
  window.location.href = '/index.html';
});

loadUser();
