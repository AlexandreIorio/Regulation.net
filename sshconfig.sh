#!/bin/bash

# Vérification si le nombre de paramètres attendu est correct
if [ "$#" -ne 2 ]; then
    echo "Utilisation: $0 [utilisateur] [adresse_ip_du_serveur]"
    exit 1
fi

USERNAME=$1  # Le nom d'utilisateur sur le serveur, passé en premier argument
SERVER_IP=$2  # L'adresse IP du serveur, passé en deuxième argument

# Copie de la clé publique SSH sur le serveur
ssh-copy-id ${USERNAME}@${SERVER_IP}

# Connexion au serveur pour configurer l'authentification par clé SSH
ssh ${USERNAME}@${SERVER_IP} << 'EOF'
# Création du répertoire .ssh si nécessaire
mkdir -p ~/.ssh
chmod 700 ~/.ssh

# Configuration de l'authentification par clé SSH dans sshd_config, si nécessaire
grep -qxF 'PubkeyAuthentication yes' /etc/ssh/sshd_config || echo 'PubkeyAuthentication yes' | tee -a /etc/ssh/sshd_config > /dev/null
grep -qxF 'PasswordAuthentication no' /etc/ssh/sshd_config || echo 'PasswordAuthentication no' | tee -a /etc/ssh/sshd_config > /dev/null

# Redémarrage du service SSH pour appliquer les changements
sudo systemctl restart sshd
EOF

echo "Configuration SSH terminée sur ${SERVER_IP}"

