# -*- coding: utf-8 -*-

import mysql.connector
from datetime import datetime

def connect_to_database():
    return mysql.connector.connect(
        host="localhost",  # Altere conforme necessário
        user="root",       # Altere conforme necessário
        password="toor",  # Altere conforme necessário
        database="FindIT"
    )

def populate_database():
    connection = connect_to_database()
    cursor = connection.cursor()

    try:
        users = [
            ("Alice", "alice@example.com", "hashedpassword1", "123.456.789-00", True),
            ("Bob", "bob@example.com", "hashedpassword2", "987.654.321-00", True),
            ("Charlie", "charlie@example.com", "hashedpassword3", "111.222.333-44", False)
        ]
        cursor.executemany(
            "INSERT INTO user (name, email, password, cpf, is_active) VALUES (%s, %s, %s, %s, %s)",
            users
        )

        categories = [
            ("Electronics", True),
            ("Books", True),
            ("Clothing", False)
        ]
        cursor.executemany(
            "INSERT INTO category (name, is_active) VALUES (%s, %s)",
            categories
        )

        action_types = [
            ("Registered",),
            ("Claimed",),
            ("Updated",)
        ]
        cursor.executemany(
            "INSERT INTO action_type (description) VALUES (%s)",
            action_types
        )

        items = [
            ("Laptop", "Dell XPS 15", True, 1, 1, 2),
            ("Python Book", "Learn Python Programming", True, 2, 2, 3),
            ("Jacket", "Winter Jacket", False, 3, 3, 1)
        ]
        cursor.executemany(
            "INSERT INTO item (name, description, is_active, category_id, registered_by, claimed_by) VALUES (%s, %s, %s, %s, %s, %s)",
            items
        )

        permissions = [
            ("can_register_item", "Permission to register items"),
            ("can_claim_item", "Permission to claim items"),
            ("can_manage_users", "Permission to manage users")
        ]
        cursor.executemany(
            "INSERT INTO permission (`key`, description) VALUES (%s, %s)",
            permissions
        )

        user_permissions = [
            (1, 1, True),
            (2, 2, True),
            (3, 3, False)
        ]
        cursor.executemany(
            "INSERT INTO user_permission (user_id, permission_id, is_active) VALUES (%s, %s, %s)",
            user_permissions
        )

        item_history = [
            (1, 1, 1, "Laptop", "Dell XPS 15", datetime.now(), datetime.now(), True, 1, 1, 2),
            (2, 2, 2, "Python Book", "Learn Python Programming", datetime.now(), datetime.now(), True, 2, 2, 3),
            (3, 3, 3, "Jacket", "Winter Jacket", datetime.now(), datetime.now(), False, 3, 3, 1)
        ]
        cursor.executemany(
            """INSERT INTO item_history (
                user_id, action_type, item_id, item_name, item_description, item_created_at,
                item_updated_at, item_is_active, item_category_id, item_registered_by, item_claimed_by
            ) VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)""",
            item_history
        )

        connection.commit()
        print("Banco de dados populado com sucesso!")

    except mysql.connector.Error as err:
        print(f"Erro: {err}")
        connection.rollback()
    finally:
        cursor.close()
        connection.close()

if __name__ == "__main__":
    populate_database()
