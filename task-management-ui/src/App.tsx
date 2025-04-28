import TaskList from "./components/Task/TaskList";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import Layout from "./components/Global/Layout";
import About from "./components/Global/About";
import TaskDetail from "./components/Task/TaskDetail";
import AddTask from "./components/Task/AddTask";
import UpdateTask from "./components/Task/UpdateTask";
import LandingPage from "./components/Global/LandingPage";
import { useAuthStore } from "./store/useAuthStore";
import RegisterForm from "./components/Auth/RegisterForm";
import LoginForm from "./components/Auth/LoginForm";
import { useEffect } from "react";
import NotificationList from "./components/Notification/NotificationList";
import useSignalR from "./store/useSignalR";

const App: React.FC = () => {
  const { isAuthenticated, userIsAuthenticated } = useAuthStore();
  useSignalR();

  useEffect(() => {
    userIsAuthenticated();
  }, [userIsAuthenticated]);

  return (
    <Router>
      <Routes>
        <Route
          path="/"
          element={isAuthenticated ? <Layout /> : <LandingPage />}
        >
          {isAuthenticated && (
            <>
              <Route index element={<TaskList />}></Route>
              <Route path="tasks/:id" element={<TaskDetail />}></Route>
              <Route path="about" element={<About />}></Route>
              <Route path="tasks/add" element={<AddTask />} />
              <Route path="tasks/edit/:id" element={<UpdateTask />} />
              <Route path="notifications" element={<NotificationList />} />
            </>
          )}
          <Route path="*" element={<Navigate to="/" />} />
        </Route>
        <Route path="register" element={<RegisterForm />} />
        <Route path="login" element={<LoginForm />} />
      </Routes>
    </Router>
  );
};

export default App;
