import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './index.css';
import App from './App';
import MailService from '@sendgrid/mail';
import reportWebVitals from './reportWebVitals';
import { ApplicationPaths } from './components/signforms/api-authorization/ApiAuthorizationConstants';
import ApiAuthorizationRoutes from './components/signforms/api-authorization/ApiAuthorizationRoutes';

ReactDOM.render(
    <BrowserRouter>
        <Routes>
            <Route path='/' element={<App />} />
            <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
            {/* <AuthorizeRoute path='/fetch-data' component={FetchData} /> */}
        </Routes>
    </BrowserRouter>,
    document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals(console.log);

// SendGrid
MailService.setApiKey(process.env.SENDGRID_API_KEY);
// TODO: msg