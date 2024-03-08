-- =======================================================
CREATE TABLE user_info (
    id INT GENERATED ALWAYS AS IDENTITY,
	name VARCHAR(64) NOT NULL,
	email VARCHAR(64),
	created_at TIMESTAMP NOT NULL DEFAULT NOW(),
	last_login TIMESTAMP,
	is_active BOOL NOT NULL DEFAULT true,
	PRIMARY KEY(id)
	
);

-- DROP TABLE user_info;

-- =======================================================
CREATE TABLE user_credentials (
    id INT GENERATED ALWAYS AS IDENTITY,
	user_id INT,
	username VARCHAR(64),
	password VARCHAR(256),
	salt VARCHAR(256),
	PRIMARY KEY(id),
	CONSTRAINT fk_user_info
	    FOREIGN KEY(user_id)
	    REFERENCES user_info(id)
	    ON DELETE CASCADE
	    ON UPDATE CASCADE
)

-- DROP TABLE user_credentials;


-- =======================================================
CREATE EXTENSION IF NOT EXISTS pgcrypto;


-- =======================================================
CREATE TYPE type_Hash AS (
    password VARCHAR(256),
    salt VARCHAR(256)
);


-- =======================================================
CREATE OR REPLACE FUNCTION ard_hash_password(p_password VARCHAR(128))
RETURNS SETOF type_Hash AS $$
DECLARE v_generated_salt VARCHAR(256);
BEGIN
	v_generated_salt := gen_salt('bf');
    RETURN QUERY SELECT crypt(p_password, v_generated_salt)::VARCHAR(256) AS password, v_generated_salt AS salt;
END;
$$ LANGUAGE plpgsql;


-- =======================================================
CREATE OR REPLACE FUNCTION ard_user_exist(
    p_username VARCHAR(64)
)
RETURNS INT AS $$
DECLARE
    v_id INT;
BEGIN
    SELECT COUNT(*) INTO v_id FROM user_credentials 
	WHERE username = p_username;
	
	RETURN v_id;
END;
$$ LANGUAGE plpgsql;

-- =======================================================
CREATE OR REPLACE FUNCTION ard_create_user(
    p_username VARCHAR(64),
    p_password VARCHAR(128),
	p_name VARCHAR(64),
	p_email VARCHAR(64),
	p_created_at TIMESTAMP DEFAULT NOW(),
	p_is_active BOOL DEFAULT true
)
RETURNS INT AS $$
DECLARE
    v_id INT;
	v_hash type_Hash;
BEGIN
    INSERT INTO user_info (name, email, created_at, is_active)
    VALUES (p_name, p_email, p_created_at, p_is_active)
    RETURNING id INTO v_id;

	SELECT * INTO v_hash FROM ard_hash_password(p_password); 
	
    INSERT INTO user_credentials (user_id, username, password, salt)
    VALUES (v_id, p_username, v_Hash.password, v_Hash.salt);

    RETURN v_id;
END;
$$ LANGUAGE plpgsql;



-- =======================================================
-- If given info has exactly one entry, returns user_id
-- Otherwise returns 0
CREATE OR REPLACE FUNCTION ard_validate_creds(
    p_username VARCHAR(64),
	p_password VARCHAR(128)
)
RETURNS INT AS $$
DECLARE
    v_id INT;
	v_count INT;
BEGIN
    SELECT COUNT(*) INTO v_count FROM user_credentials 
	WHERE 
	    username = p_username 
		AND password = crypt(p_password, salt);
	
    IF v_count = 0 OR v_count > 1 THEN
        RETURN 0;
	END IF;
	
    SELECT user_id INTO v_id FROM user_credentials 
    WHERE 
        username = p_username 
        AND password = crypt(p_password, salt)::VARCHAR(256);
	
	RETURN v_id;
END;
$$ LANGUAGE plpgsql;


-- =======================================================
-- Reset Password
CREATE OR REPLACE FUNCTION ard_reset_password (
    p_username VARCHAR(64),
	p_password VARCHAR(128)
)
RETURNS BOOL AS $$
DECLARE
    v_id INT;
	v_count INT;
	v_hash type_Hash;
BEGIN
    v_count := ard_user_exist(p_username);
	
    IF v_count = 0 OR v_count > 1 THEN
        RETURN false;
	END IF;
	
	SELECT * INTO v_hash FROM ard_hash_password(p_password);
	
    UPDATE user_credentials
	SET password = v_hash.password
    WHERE username = p_username;
	
	UPDATE user_credentials
	SET salt = v_hash.salt
    WHERE username = p_username;
	
	RETURN true;
END;
$$ LANGUAGE plpgsql;
