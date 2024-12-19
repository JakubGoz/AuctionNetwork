import authClasses from '../../AuthenticationPage.module.scss';
import { useState, useRef } from "react";
import axios from "axios";
import { baseUrl } from '../../../Shared/Options/ApiOptions';

function PasswordResetComponent(props) {
    const emailErrRef = useRef(null);
    const tokenErrRef = useRef(null);
    const passwordErrRef = useRef(null);

    const [IsVerified, setIsVerified] = useState(false);
    const [emailState, setEmailState] = useState("");

    const handleVerify = async (e) => {
        e.preventDefault();
        const email = e.target.email.value.trim();

        // Check for valid email format
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (email === "") {
            emailErrRef.current.innerHTML = "Email can not be empty.";
            return;
        } else if (!emailRegex.test(email)) {
            emailErrRef.current.innerHTML = "Email is not valid.";
            return;
        } else {
            emailErrRef.current.innerHTML = "";
        }

        const data = {
            email: email
        }

        try {
            await axios.post(`${baseUrl}/user/verify-password-reset`, data);
            setEmailState(data.email);
            setIsVerified(true);
        } catch (err) {
            // Display backend exeptions
            if (err.response && err.response.data) {
                emailErrRef.current.innerHTML = err.response.data;
            }
        }
    };

    const handleReset = async (e) => {
        e.preventDefault();

        const data = {
            email: emailState,
            token: e.target.token.value,
            password: e.target.password.value,
        }

        // Regex for password validation (minimum 8 characters, one uppercase, one lowercase, one digit, one special character)
        const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$/;

        let hasErrors = false;

        // Validate token
        if (!data.token) {
            tokenErrRef.current.innerHTML = "Token is required.";
            hasErrors = true;
        } else {
            tokenErrRef.current.innerHTML = "";
        }

        // Validate password
        if (!passwordRegex.test(data.password)) {
            passwordErrRef.current.innerHTML = "Password must be at least 8 characters long, with at least one uppercase, one lowercase, one digit, and one special character.";
            hasErrors = true;
        } else {
            passwordErrRef.current.innerHTML = "";
        }

        if (hasErrors) {
            return;
        }

        try {
            await axios.post(`${baseUrl}/user/password-reset`, data);
            handleLogIn();
            alert("New password was set. You can sign in now.");
        } catch (err) {
            // Display backend exceptions
            if (err.response && err.response.data) {
                tokenErrRef.current.innerHTML = err.response.data;
            }
        }
    };

    const handleCancel = (e) => {
        setIsVerified(false);
    };

    const handleLogIn = (e) => {
        props.changeShowedComponent("Login")
    };

    return (
        <>
            {!IsVerified && (
                <>
                    <h2 className={authClasses['auth-header']}>Reset password</h2>
                    <form onSubmit={handleVerify} className={authClasses['auth-form']}>
                        <div className={authClasses['auth-input-group']}>
                            <label className={authClasses['auth-label']} htmlFor="email">Email:</label>
                            <input className={authClasses['auth-input']} type="text" id="email" name="email" placeholder="Enter email" />
                            <span ref={emailErrRef} className={authClasses['auth-error-message']}></span>
                        </div>
                        <div className='row justify-content-between'>
                            <div className='col-5'>
                                <button onClick={handleLogIn} className={authClasses['auth-primary-btn']}>
                                    Cancel
                                </button>
                            </div>
                            <div className='col-5'>
                                <button type='submit' className={authClasses['auth-primary-btn']}>
                                    Submit
                                </button>
                            </div>
                        </div>
                    </form>
                </>
            )}
            {IsVerified && (
                <>
                    <h2 className={authClasses['auth-header']}>Reset password</h2>
                    <form onSubmit={handleReset} className={authClasses['auth-form']}>
                        <div className={authClasses['auth-input-group']}>
                            <label className={authClasses['auth-label']} htmlFor="token">We have sent a verification token to the provided email. Token will expire in 5 minutes.</label>
                            <input className={authClasses['auth-input']} type="number" id="token" name="token" placeholder="Enter token" />
                            <span ref={tokenErrRef} className={authClasses['auth-error-message']}></span>
                        </div>
                        <div className={authClasses['auth-input-group']}>
                            <label className={authClasses['auth-label']} htmlFor="password">Password</label>
                            <input className={authClasses['auth-input']} type="password" id="password" name="password" placeholder="Enter password" />
                            <span ref={passwordErrRef} className={authClasses['auth-error-message']}></span>
                        </div>
                        <div className='row justify-content-between'>
                            <div className='col-5'>
                                <button onClick={handleCancel} className={authClasses['auth-primary-btn']}>
                                    Cancel
                                </button>
                            </div>
                            <div className='col-5'>
                                <button type='submit' className={authClasses['auth-primary-btn']}>
                                    Set new password
                                </button>
                            </div>
                        </div>
                    </form>
                </>
            )}
        </>
    );
}

export default PasswordResetComponent; 