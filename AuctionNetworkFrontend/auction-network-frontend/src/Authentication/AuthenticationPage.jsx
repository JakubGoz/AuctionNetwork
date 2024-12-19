import classes from './AuthenticationPage.module.scss';
import LoginComponent from './Components/LoginComponent/LoginComponent';
import RegisterComponent from './Components/RegisterComponent/RegisterComponent';
import { useState } from 'react';

function AuthenticationPage() {
    const [ShowedComponent, setShowedComponent] = useState("Login");

    const changeShowedComponent = (componentName) => {
        setShowedComponent(componentName);
    }

    return (
        <div className={classes['auth-container']}>
            {ShowedComponent === "Login" && (
                <LoginComponent
                    changeShowedComponent={changeShowedComponent}
                />
            )}
            {ShowedComponent === "Register" && (
                <RegisterComponent
                    changeShowedComponent={changeShowedComponent}
                />
            )}
            {ShowedComponent === "PasswordReset" && (
                <LoginComponent
                    changeShowedComponent={changeShowedComponent}
                />
            )}
        </div>
    );
}

export default AuthenticationPage;