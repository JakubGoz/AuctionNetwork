import authClasses from '../../AuthenticationPage.module.scss';
import { useState, useRef } from "react";
import axios from "axios";
import { baseUrl } from '../../../Shared/Options/ApiOptions';

function LoginComponent(props) {
    const emailErrRef = useRef(null);
    const passwordErrRef = useRef(null);
    const tokenErrRef = useRef(null);

    const [IsLoggedIn, setIsLoggedIn] = useState(false);
    const [EmailAndPassword, setEmailAndPassword] = useState(false);

    const handleLogIn = async (e) => {
        e.preventDefault();
        const userData = {
            email: e.target.email.value.trim(),
            password: e.target.password.value,
        };

        // Check for valid email format
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (userData.email === "") {
            emailErrRef.current.innerHTML = "Email can not be empty.";
            return;
        } else if (!emailRegex.test(userData.email)) {
            emailErrRef.current.innerHTML = "Email is not valid.";
            return;
        } else {
            emailErrRef.current.innerHTML = "";
        }

        // Check for empty password
        if (userData.password.length === 0) {
            passwordErrRef.current.innerHTML = "Password can not be empty.";
            return;
        } else {
            passwordErrRef.current.innerHTML = "";
        }

        try {

            await axios.post(`${baseUrl}/user/login`, userData);
            setEmailAndPassword(userData);
            setIsLoggedIn(true);
          } catch (err) {

            if (err.response && err.response.data) {
                emailErrRef.current.innerHTML = err.response.data;
            }
          }
    };

    const handleVerify = async (e) => {
        e.preventDefault();
        const data = {
            email: EmailAndPassword.email,
            password: EmailAndPassword.password,
            token: e.target.token.value
        };

        try {

            const response = await axios.post(`${baseUrl}/user/verify-login`, data);
            localStorage.setItem("token", response.data.token);
            alert("You are logged in");

          } catch (err) {
            if (err.response && err.response.data) {
                tokenErrRef.current.innerHTML = err.response.data;
            }
          }
    };

    const handleCancel = (e) => {
        setIsLoggedIn(false);
    };

    const handleRegister = (e) => {
        props.changeShowedComponent("Register")
    };

    const handleResetPassword = (e) => {
        props.changeShowedComponent("ResetPassword")
    };

    return (
        <>
            {!IsLoggedIn && (
                <>
                    <h2 className={authClasses['auth-header']}>Sign in</h2>
                    <form onSubmit={handleLogIn} className={authClasses['auth-form']}>
                        <div className={authClasses['auth-input-group']}>
                            <label className={authClasses['auth-label']} htmlFor="email">Email:</label>
                            <input className={authClasses['auth-input']} type="text" id="email" name="email" placeholder="Enter email" />
                            <span ref={emailErrRef} className={authClasses['auth-error-message']}></span>
                        </div>
                        <div className={authClasses['auth-input-group']}>
                            <label className={authClasses['auth-label']} htmlFor="password">Password:</label>
                            <input className={authClasses['auth-input']} type="password" id="password" name="password" placeholder="Enter password" />
                            <span ref={passwordErrRef} className={authClasses['auth-error-message']}></span>
                        </div>
                        <button type='submit' className={authClasses['auth-primary-btn']}>
                            Sign in
                        </button>
                    </form>
                    <div className={authClasses['auth-actions']}>
                        <button onClick={handleRegister} className={authClasses['auth-secondary-btn']}>Sign up</button>
                        <button onClick={handleResetPassword} className={authClasses['auth-secondary-btn']}>Reset password</button>
                    </div>
                </>
            )}
            {IsLoggedIn && (
                <>
                    <h2 className={authClasses['auth-header']}>Email verification</h2>
                    <form onSubmit={handleVerify} className={authClasses['auth-form']}>
                        <div className={authClasses['auth-input-group']}>
                            <label className={authClasses['auth-label']} htmlFor="token">We have sent a verification token to the provided email. Token will expire in 5 minutes.</label>
                            <input className={authClasses['auth-input']} type="number" id="token" name="token" placeholder="Enter token" />
                            <span ref={tokenErrRef} className={authClasses['auth-error-message']}></span>
                        </div>
                        <div className='row justify-content-between'>
                            <div className='col-5'>
                                <button onClick={handleCancel} className={authClasses['auth-primary-btn']}>
                                    Cancel
                                </button>
                            </div>
                            <div className='col-5'>
                                <button type='submit' className={authClasses['auth-primary-btn']}>                                    Verify
                                </button>
                            </div>
                        </div>
                    </form>
                </>
            )}
        </>
    );
}

export default LoginComponent;  