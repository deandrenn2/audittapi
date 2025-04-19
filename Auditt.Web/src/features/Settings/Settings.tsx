import { LinkSettings } from "../Dashboard/LinkSenttings"

export const Settings = () => {
    return (
        <div className="container-fluid">
            <div className="row">
                <div className="col-12">
                     <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                        <LinkSettings />
                    </div>
                </div>
            </div>
        </div>
    )
}