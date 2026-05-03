# Certificates

`rootCA.pem` is committed — it is the user's mkcert root CA used by every
.NET image to trust the edge cert. It is reused across machines that share
the same mkcert installation.

`cert.pem` and `key.pem` must be generated locally with mkcert and are
gitignored. Run from the repo root:

```
mkcert -cert-file nginx/certs/cert.pem -key-file nginx/certs/key.pem \
  identity.eshop webapp.eshop basket.eshop catalog.eshop ordering.eshop
```
